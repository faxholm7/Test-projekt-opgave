using MitForbrug.Interfaces.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitForbrug_Tests.Fakes.WebAPI
{
    public class AbstractApiCallsFake : IAbstractApiCalls
    {
        public string EarlistMeasurementsJSON { get; set; }
        public string LastMeasurementsJSON { get; set; }
        public string MeasurementsJSON { get; set; }
        public string SignalsJSON { get; set; } 

        public string GetEarliestMeasurementBySignalId(int signalId, DateTime sinceDate)
        {
            var measurements = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Measurement>>(EarlistMeasurementsJSON);
            var measurement = measurements.Where(x => x.SignalID == signalId && x.CreatedAt.Date >= sinceDate.Date);
            return Newtonsoft.Json.JsonConvert.SerializeObject(measurement);
        }

        public string GetSignalsByMeterIdAndSignalType(int meterId, int signaltype)
        {
            var signals = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Signal>>(SignalsJSON);
            var signal = signals.Where(x => x.MeterID == meterId && x.SignalType == signaltype);
            return Newtonsoft.Json.JsonConvert.SerializeObject(signal);
        }

        public string GetLastMeasurementBySignalId(int signalId, DateTime sinceDate)
        {
            var measurements = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Measurement>>(LastMeasurementsJSON);
            var measurement = measurements.Where(x => x.SignalID == signalId && x.CreatedAt.Date >= sinceDate.Date);
            return Newtonsoft.Json.JsonConvert.SerializeObject(measurement);
        }

        public string GetMeasurementsBySignalId(int signalId, DateTime fromDate, DateTime toDate)
        {
            var allMeasurements = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Measurement>>(MeasurementsJSON);
            var measurements = allMeasurements.Where(x => x.SignalID == signalId && x.CreatedAt.Date >= fromDate.Date && x.CreatedAt.Date <= toDate.Date);
            return Newtonsoft.Json.JsonConvert.SerializeObject(measurements);
        }


        public int CustomerInterfaceType => throw new NotImplementedException();
        public string GetContractsByPersonGuid(string personGuid)
        {
            throw new NotImplementedException();
        }

        public string GetJSONfromApiRequest(string urlPart, string body = "")
        {
            throw new NotImplementedException();
        }



        public string GetLeaseByNumber(int personNumberId)
        {
            throw new NotImplementedException();
        }

        public string GetLeaseInfoByAddress(string address)
        {
            throw new NotImplementedException();
        }

        public string GetLeaseInfoByGroupGUID(string groupGUID)
        {
            throw new NotImplementedException();
        }

        public string GetLeaseInfoByGroupNumbers(string companyNumber, string divisionNumber, string leaseNumber)
        {
            throw new NotImplementedException();
        }

        public string GetLoginByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public string GetLoginByPersonIdAndPassword(int personId, string password)
        {
            throw new NotImplementedException();
        }

        public string GetMeasurementsBySignalId(int signalId, DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public string GetMetersByLeaseGuid(string leaseGroupGUID)
        {
            throw new NotImplementedException();
        }

        public string GetMetersByLeaseGuidAndMediumId(string leaseGroupGUID, int deviceTypeId)
        {
            throw new NotImplementedException();
        }

        public string GetPersonsByNumber(int personNumber)
        {
            throw new NotImplementedException();
        }

        public string GetPrimaryTenantNumberByAdressAndOtherName(string adress, string othername)
        {
            throw new NotImplementedException();
        }



        public void SetCustomerInterfaceType(int cit)
        {
            throw new NotImplementedException();
        }

        public void SetCustomerInterfaceType(string cit)
        {
            throw new NotImplementedException();
        }

        public void SetCustomerName(string customerName)
        {
        }
    }
}
