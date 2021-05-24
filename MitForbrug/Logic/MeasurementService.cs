using MitForbrug.Interfaces;
using MitForbrug.Interfaces.WebAPI;
using MitForbrug.Models.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static MitForbrug.Models.Enums;

namespace MitForbrug.Logic
{
    public class MeasurementService
    {
        private readonly ISessionRepository _sessionRepository;

        public MeasurementService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public IDictionary<string, List<MeterWithMeasurementsViewModel>> GetMeasurementsFromMetersComparativePeriod(List<Meter> meters, int period, DateTime leaseOccupiedAt, DateTime endDate = new DateTime(), PreviousPeriodes periodType = PreviousPeriodes.None)
        {
            IDictionary<string,List<MeterWithMeasurementsViewModel>> meterCollection = new Dictionary<string, List<MeterWithMeasurementsViewModel>>();

            meterCollection.Add(new KeyValuePair<string, List<MeterWithMeasurementsViewModel>>("thisPeriod", new List<MeterWithMeasurementsViewModel>()));
            meterCollection.Add(new KeyValuePair<string, List<MeterWithMeasurementsViewModel>>("previousPeriod", new List<MeterWithMeasurementsViewModel>()));

            var thisPeriod = new List<MeterWithMeasurementsViewModel>();
            var prevPeriod = new List<MeterWithMeasurementsViewModel>();
            var errors = new List<string>();    

            if (meters == null)
                return meterCollection;

            //Gets the flexcom from session. gets set in homecontroller index 
            var apiFlexcomData = _sessionRepository.GetFlexComAPI();
            apiFlexcomData.SetCustomerName(_sessionRepository.GetFromKey("customerName").ToString());

            foreach (var meter in meters)
            {
                meter.UsedDueDate = meter.UsedDueDate.AddDays(1);
                var meterWithMeasurements = new MeterWithMeasurementsViewModel()
                {
                    Measurements = new List<Measurement>(),
                    PeriodInDays = period,
                    UsageSum = 0,
                    Meter = meter,
                    MediumName = AppSetup.mediums.Where(med => med.Id == meter.DeviceTypeID).Select(med => med.MediumName).FirstOrDefault(),
                    MediumUnit = AppSetup.mediums.Where(med => med.Id == meter.DeviceTypeID).Select(med => med.Unit).FirstOrDefault() 
                };

                var meterWithMeasurementsForpreviousPeriod = new MeterWithMeasurementsViewModel()
                {
                    Measurements = new List<Measurement>(),
                    PeriodInDays = period,
                    UsageSum = 0
                };

                if (periodType != PreviousPeriodes.None)
                {
                    meterWithMeasurementsForpreviousPeriod.Meter = meter;
                    meterWithMeasurementsForpreviousPeriod.MediumName = AppSetup.mediums.Where(med => med.Id == meter.DeviceTypeID).Select(med => med.MediumName).FirstOrDefault();
                    meterWithMeasurementsForpreviousPeriod.MediumUnit = AppSetup.mediums.Where(med => med.Id == meter.DeviceTypeID).Select(med => med.Unit).FirstOrDefault();
                }
                var signalJSON = apiFlexcomData.GetSignalsByMeterIdAndSignalType(meter.MeterID, 1);
                var signals = JsonConvert.DeserializeObject<List<Signal>>(signalJSON,
                    new JsonSerializerSettings
                    {
                        Error = delegate (object sender, ErrorEventArgs args)
                        {
                            errors.Add(args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        },
                        Converters = 
                        { 
                            new IsoDateTimeConverter() 
                        }
                    });

                var dateBackInTime = new DateTime();

                if(endDate.Date == dateBackInTime.Date)
                {
                    dateBackInTime = DateTime.Now.Date.AddDays(meterWithMeasurements.PeriodInDays * -1);
                    endDate = DateTime.Now;
                }
                else
                    dateBackInTime = endDate.Date.AddDays(meterWithMeasurements.PeriodInDays * -1);

                meterWithMeasurements.StartDate = dateBackInTime.ToShortDateString();
                meterWithMeasurements.EndDate = endDate.ToShortDateString();

                if (dateBackInTime < leaseOccupiedAt)
                    dateBackInTime = leaseOccupiedAt;

                //same for dateback in time
                var endDateInPreviousPeriod = DateTime.Now;

                switch (periodType)
                {
                    case PreviousPeriodes.CustomPeriod:
                        endDateInPreviousPeriod = dateBackInTime;
                        break;
                    case PreviousPeriodes.Week:
                        endDateInPreviousPeriod = endDate.AddDays(-7);
                        break;
                    case PreviousPeriodes.Month:
                        endDateInPreviousPeriod = endDate.AddMonths(-1);
                        break;
                    case PreviousPeriodes.Year:
                        endDateInPreviousPeriod = endDate.AddYears(-1);
                        break;
                }

                var dateBackInTimePreviousPeriod = endDateInPreviousPeriod.AddDays(meterWithMeasurements.PeriodInDays * -1);

                if (periodType != PreviousPeriodes.None)
                {
                    meterWithMeasurementsForpreviousPeriod.EndDate = endDateInPreviousPeriod.ToShortDateString();
                    meterWithMeasurementsForpreviousPeriod.StartDate = dateBackInTimePreviousPeriod.ToShortDateString();
                }

                if (dateBackInTimePreviousPeriod < leaseOccupiedAt)
                    dateBackInTimePreviousPeriod = leaseOccupiedAt;

                foreach (var signal in signals)
                {
                    var measurementJson = apiFlexcomData.GetEarliestMeasurementBySignalId(signal.SignalID, leaseOccupiedAt);
                    var measurement = JsonConvert.DeserializeObject<List<Measurement>>(measurementJson);
                    if (measurement.Count > 0)
                        meterWithMeasurements.EarliestMeasurement = measurement.First().MeasuredAt;

                    var measurements = GetMeasurements(apiFlexcomData, signal.SignalID, dateBackInTime, endDate);

                    //TODO maybe the last and first measurement could be fetched directely from the API?
                    if (measurements != null)
                        meterWithMeasurements.UsageSum += SumOfMeasurements(measurements, meter, dateBackInTime);

                    if (periodType != PreviousPeriodes.None)
                    {
                        var measurementsForPreviousPeriod = GetMeasurements(apiFlexcomData, signal.SignalID, dateBackInTimePreviousPeriod, endDateInPreviousPeriod);

                        if (measurementsForPreviousPeriod != null)
                            meterWithMeasurementsForpreviousPeriod.UsageSum += SumOfMeasurements(measurementsForPreviousPeriod, meter, dateBackInTimePreviousPeriod);
                    }
                }

                thisPeriod.Add(meterWithMeasurements);

                if(meterWithMeasurementsForpreviousPeriod.Meter != null)
                    prevPeriod.Add(meterWithMeasurementsForpreviousPeriod);
            }
            meterCollection["thisPeriod"] = thisPeriod;
            meterCollection["previousPeriod"] = prevPeriod;

            return meterCollection;
        }

     
       
        private int SumOfMeasurements(List<Measurement> measurementsForPeriod, Meter meter, DateTime dateBackInTime)
        {
            var measurementsOverPeriod = measurementsForPeriod.Where(m => m.MeasuredAt >= dateBackInTime).Select(m => m).ToList();
            if(measurementsOverPeriod.Count() == 0)
                return 0;

            int unitCorrector = 1;

            if (meter.DeviceTypeID == 6 || meter.DeviceTypeID == 7)
                unitCorrector = 1000;

            int sumOfMeasurements = 0;
            // if the meter is resetting within the period represented, we have to add the consumption before resetting with the comsumption after resetting
            if (meter.IsResettingOnDueDate && meter.UsedDueDate > dateBackInTime && meter.UsedDueDate <= measurementsForPeriod.Last().MeasuredAt.Date)
            {
                int FirstSumOfMeasurements = (int)(Math.Round(measurementsOverPeriod.Find(x => x.MeasuredAt.Date == meter.ReportedDueDate.Value.Date).Value - measurementsOverPeriod.First().Value, 4) * unitCorrector);
                // For the last part we can just take the raw value, as we know it started with zero
                int SecondSumOfMeasurements = (int)(Math.Round(measurementsOverPeriod.Last().Value, 4) * unitCorrector);
                sumOfMeasurements = FirstSumOfMeasurements + SecondSumOfMeasurements;
            }
            else // just a standard measurement period.
            {
                var sumtest = (measurementsOverPeriod.Last().Value - measurementsOverPeriod.First().Value) * unitCorrector;
                sumOfMeasurements = (int)(Math.Round(measurementsOverPeriod.Last().Value - measurementsOverPeriod.First().Value, 4) * unitCorrector);
            }
            return sumOfMeasurements;
        }

        private List<Measurement> GetMeasurements(IAbstractApiCalls apiFlexcomData, int SignalID, DateTime dateBackInTime, DateTime dateto)
        {
            //get measurements between dates
            var newestmeasurementstring = apiFlexcomData.GetLastMeasurementBySignalId(SignalID, dateto); //Last measurement
            var measurement = JsonConvert.DeserializeObject<List<Measurement>>(newestmeasurementstring);


            var jsonMeasurement = apiFlexcomData.GetMeasurementsBySignalId(SignalID, dateBackInTime, dateto);
            var measurements = JsonConvert.DeserializeObject<List<Measurement>>(jsonMeasurement);
            //if measurements har null return empty and handle in view
            if (measurements == null)
                return new List<Measurement>();

            while (measurements.Last().MeasuredAt != measurement.First().MeasuredAt)
            {
                var newMeasurements = JsonConvert.DeserializeObject<List<Measurement>>(apiFlexcomData.GetMeasurementsBySignalId(SignalID, measurements.Last().MeasuredAt, dateto));

                if (newMeasurements.Last().MeasuredAt == measurements.Last().MeasuredAt)
                    return measurements;

                measurements.AddRange(newMeasurements);
            }

            return measurements;
        }

        private AbstractApiCalls GetFlexComAPI()
        {
            var apiFlexcomData = (AbstractApiCalls)HttpContext.Current.Session["PersonAndMetersAPI"];
            apiFlexcomData.SetCustomerName(HttpContext.Current.Session["customerName"].ToString());
            return apiFlexcomData;
        }
    }
}
