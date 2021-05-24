using Microsoft.VisualStudio.TestTools.UnitTesting;
using MitForbrug.Interfaces.WebAPI;
using MitForbrug.Models.ViewModel;
using MitForbrug_Tests.Fakes.WebAPI;
using MitForbrug_Tests.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MitForbrug.Models.Enums;

namespace MitForbrug_Tests.Services.MeasurementService
{
    [TestClass]
    public class GetMeasurementsFromMetersComparativePeriodTests
    {
        [TestMethod]
        public void No_comparative_period()
        {
            //Arrange
            var flexComApiFake = new AbstractApiCallsFake()
            {
                EarlistMeasurementsJSON = GetJSONFromFile(@"No_comparative_period\EarlistMeasurement.JSON"),
                LastMeasurementsJSON = GetJSONFromFile(@"No_comparative_period\LastMeasurement.JSON"),
                MeasurementsJSON = GetJSONFromFile(@"No_comparative_period\Measurement.JSON"),
                SignalsJSON = GetJSONFromFile(@"No_comparative_period\Signals.JSON")
            };
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);
            
            var inputMeters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meter>>(GetJSONFromFile(@"No_comparative_period\InputMeters.JSON"));
            var inputPeriodLength = 23;
            var inputLeaseOccupiedAt = new DateTime(2018,03,01);
            var inputEndDate = new DateTime(2021, 05, 24);
            var inputPeriodType = PreviousPeriodes.None;

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt, inputEndDate, inputPeriodType);

            //Assert
            var expectedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, List<MeterWithMeasurementsViewModel>>>(GetJSONFromFile(@"No_comparative_period\Output.JSON"));

            AssertResult(expectedResult, result);
        }

        [TestMethod]
        public void One_week_comparative_period()
        {
            var flexComApiFake = new AbstractApiCallsFake()
            {
                EarlistMeasurementsJSON = GetJSONFromFile(@"One_week_comparative_period\EarlistMeasurements.JSON"),
                LastMeasurementsJSON = GetJSONFromFile(@"One_week_comparative_period\LastMeasurement.JSON"),
                MeasurementsJSON = GetJSONFromFile(@"One_week_comparative_period\Measurement.JSON"),
                SignalsJSON = GetJSONFromFile(@"One_week_comparative_period\Signals.JSON")
            };
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);

            var inputMeters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meter>>(GetJSONFromFile(@"One_week_comparative_period\InputMeters.JSON"));
            var inputPeriodLength = 5;
            var inputLeaseOccupiedAt = new DateTime(2018, 03, 01);
            var inputEndDate = new DateTime(2021, 05, 06);
            var inputPeriodType = PreviousPeriodes.Week;

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt, inputEndDate, inputPeriodType);

            //Assert
            var expectedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, List<MeterWithMeasurementsViewModel>>>(GetJSONFromFile(@"One_week_comparative_period\Output.JSON"));

            AssertResult(expectedResult, result);
        }

        [TestMethod]
        public void One_month_comparative_period()
        {
            var flexComApiFake = new AbstractApiCallsFake()
            {
                EarlistMeasurementsJSON = GetJSONFromFile(@"One_month_comparative_period\EarlistMeasurements.JSON"),
                LastMeasurementsJSON = GetJSONFromFile(@"One_month_comparative_period\LastMeasurement.JSON"),
                MeasurementsJSON = GetJSONFromFile(@"One_month_comparative_period\Measurement.JSON"),
                SignalsJSON = GetJSONFromFile(@"One_month_comparative_period\Signals.JSON")
            };
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);

            var inputMeters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meter>>(GetJSONFromFile(@"One_month_comparative_period\InputMeters.JSON"));
            var inputPeriodLength = 23;
            var inputLeaseOccupiedAt = new DateTime(2018, 03, 01);
            var inputEndDate = new DateTime(2021,05,24);
            var inputPeriodType = PreviousPeriodes.Month;

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt, inputEndDate, inputPeriodType);

            //Assert
            var expectedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, List<MeterWithMeasurementsViewModel>>>(GetJSONFromFile(@"One_month_comparative_period\Output.JSON"));

            AssertResult(expectedResult, result);
        }

        [TestMethod]
        public void One_year_comparative_period()
        {
            var flexComApiFake = new AbstractApiCallsFake()
            {
                EarlistMeasurementsJSON = GetJSONFromFile(@"One_year_comparative_period\EarlistMeasurements.JSON"),
                LastMeasurementsJSON = GetJSONFromFile(@"One_year_comparative_period\LastMeasurement.JSON"),
                MeasurementsJSON = GetJSONFromFile(@"One_year_comparative_period\Measurement.JSON"),
                SignalsJSON = GetJSONFromFile(@"One_year_comparative_period\Signals.JSON")
            };
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);

            var inputMeters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meter>>(GetJSONFromFile(@"One_year_comparative_period\InputMeters.JSON"));
            var inputPeriodLength = 23;
            var inputLeaseOccupiedAt = new DateTime(2018, 03, 01);
            var inputEndDate = new DateTime(2021, 05, 24);
            var inputPeriodType = PreviousPeriodes.Year;

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt, inputEndDate, inputPeriodType);

            //Assert
            var expectedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, List<MeterWithMeasurementsViewModel>>>(GetJSONFromFile(@"One_year_comparative_period\Output.JSON"));

            AssertResult(expectedResult, result);
        }

        [TestMethod]
        public void Custom_comparative_period()
        {
            var flexComApiFake = new AbstractApiCallsFake()
            {
                EarlistMeasurementsJSON = GetJSONFromFile(@"Custom_comparative_period\EarlistMeasurement.JSON"),
                LastMeasurementsJSON = GetJSONFromFile(@"Custom_comparative_period\LastMeasurement.JSON"),
                MeasurementsJSON = GetJSONFromFile(@"Custom_comparative_period\Measurement.JSON"),
                SignalsJSON = GetJSONFromFile(@"Custom_comparative_period\Signals.JSON")
            };
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);

            var inputMeters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meter>>(GetJSONFromFile(@"Custom_comparative_period\InputMeters.JSON"));
            var inputPeriodLength = 23;
            var inputLeaseOccupiedAt = new DateTime(2018, 03, 01);
            var inputEndDate = new DateTime(2021,05,24);
            var inptPeriodType = PreviousPeriodes.CustomPeriod;

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt, inputEndDate, inptPeriodType);

            //Assert
            var expectedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, List<MeterWithMeasurementsViewModel>>>(GetJSONFromFile(@"Custom_comparative_period\Output.JSON"));

            AssertResult(expectedResult, result);
        }

        [TestMethod]
        public void Meters_null()
        {
            //Arrange
            var flexComApiFake = new AbstractApiCallsFake();
            var sessioRepositoryStub = new SessionRepositoryFake()
            {
                FlexComAPI = flexComApiFake
            };
            var service = new MitForbrug.Logic.MeasurementService(sessioRepositoryStub);


            List<Meter> inputMeters = null;
            var inputPeriodLength = 23;
            var inputLeaseOccupiedAt = new DateTime(2018, 03, 01);

            //Act
            var result = service.GetMeasurementsFromMetersComparativePeriod(inputMeters, inputPeriodLength, inputLeaseOccupiedAt);

            //Assert
            var expectedCount = 0;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedCount, result["thisPeriod"].Count);
            Assert.AreEqual(expectedCount, result["previousPeriod"].Count);
        }

        private string GetJSONFromFile(string path)
        {
            return File.ReadAllText($@".\Files\Services\MeasurementService\GetMeasurementsFromMetersComparativePeriodTests\{path}");
        }

        private void AssertResult(IDictionary<string, List<MeterWithMeasurementsViewModel>> expectedResult, IDictionary<string, List<MeterWithMeasurementsViewModel>> result)
        {
            var thisPeriod = result["thisPeriod"];
            var expectedThisPeriod = expectedResult["thisPeriod"];

            Assert.AreEqual(expectedThisPeriod.Count, thisPeriod.Count);

            for (int i = 0; i < thisPeriod.Count; i++)
            {
                Assert.AreEqual(expectedThisPeriod[i].CurrentMeasurementDate, thisPeriod[i].CurrentMeasurementDate);
                Assert.AreEqual(expectedThisPeriod[i].CurrentMeasurementValue, thisPeriod[i].CurrentMeasurementValue);
                Assert.AreEqual(expectedThisPeriod[i].DueDatesMeasurementDate, thisPeriod[i].DueDatesMeasurementDate);
                Assert.AreEqual(expectedThisPeriod[i].DueDatesMeasurementValue, thisPeriod[i].DueDatesMeasurementValue);
                Assert.AreEqual(expectedThisPeriod[i].EarliestMeasurement, thisPeriod[i].EarliestMeasurement);
                Assert.AreEqual(expectedThisPeriod[i].EndDate, thisPeriod[i].EndDate);
                Assert.AreEqual(expectedThisPeriod[i].IntervalPeriodStart, thisPeriod[i].IntervalPeriodStart);
                Assert.AreEqual(expectedThisPeriod[i].MediumUnit, thisPeriod[i].MediumUnit);
                Assert.AreEqual(expectedThisPeriod[i].PeriodInDays, thisPeriod[i].PeriodInDays);
                Assert.AreEqual(expectedThisPeriod[i].StartDate, thisPeriod[i].StartDate);
                Assert.AreEqual(expectedThisPeriod[i].UsageSum, thisPeriod[i].UsageSum);

                for (int x = 0; x < thisPeriod[i].Measurements.Count; x++)
                {
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].CreatedAt, thisPeriod[i].Measurements[x].CreatedAt);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].MeasuredAt, thisPeriod[i].Measurements[x].MeasuredAt);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].MeasurementCreationMethodType, thisPeriod[i].Measurements[x].MeasurementCreationMethodType);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].MeasurementID, thisPeriod[i].Measurements[x].MeasurementID);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].MeterStatusHistoryID, thisPeriod[i].Measurements[x].MeterStatusHistoryID);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].SignalID, thisPeriod[i].Measurements[x].SignalID);
                    Assert.AreEqual(expectedThisPeriod[i].Measurements[x].Value, thisPeriod[i].Measurements[x].Value);
                }
            }

            var previosPeriod = result["previousPeriod"];
            var expectedPreviousPeriod = expectedResult["previousPeriod"];

            for (int i = 0; i < previosPeriod.Count; i++)
            {
                Assert.AreEqual(expectedPreviousPeriod[i].CurrentMeasurementDate, previosPeriod[i].CurrentMeasurementDate);
                Assert.AreEqual(expectedPreviousPeriod[i].CurrentMeasurementValue, previosPeriod[i].CurrentMeasurementValue);
                Assert.AreEqual(expectedPreviousPeriod[i].DueDatesMeasurementDate, previosPeriod[i].DueDatesMeasurementDate);
                Assert.AreEqual(expectedPreviousPeriod[i].DueDatesMeasurementValue, previosPeriod[i].DueDatesMeasurementValue);
                Assert.AreEqual(expectedPreviousPeriod[i].EarliestMeasurement, previosPeriod[i].EarliestMeasurement);
                Assert.AreEqual(expectedPreviousPeriod[i].EndDate, previosPeriod[i].EndDate);
                Assert.AreEqual(expectedPreviousPeriod[i].IntervalPeriodStart, previosPeriod[i].IntervalPeriodStart);
                Assert.AreEqual(expectedPreviousPeriod[i].MediumName, previosPeriod[i].MediumName);
                Assert.AreEqual(expectedPreviousPeriod[i].MediumUnit, previosPeriod[i].MediumUnit);
                Assert.AreEqual(expectedPreviousPeriod[i].PeriodInDays, previosPeriod[i].PeriodInDays);
                Assert.AreEqual(expectedPreviousPeriod[i].StartDate, previosPeriod[i].StartDate);
                Assert.AreEqual(expectedPreviousPeriod[i].UsageSum, previosPeriod[i].UsageSum);

                for (int x = 0; x < thisPeriod[i].Measurements.Count; x++)
                {
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].CreatedAt, previosPeriod[i].Measurements[x].CreatedAt);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].MeasuredAt, previosPeriod[i].Measurements[x].MeasuredAt);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].MeasurementCreationMethodType, previosPeriod[i].Measurements[x].MeasurementCreationMethodType);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].MeasurementID, previosPeriod[i].Measurements[x].MeasurementID);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].MeterStatusHistoryID, previosPeriod[i].Measurements[x].MeterStatusHistoryID);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].SignalID, previosPeriod[i].Measurements[x].SignalID);
                    Assert.AreEqual(expectedPreviousPeriod[i].Measurements[x].Value, previosPeriod[i].Measurements[x].Value);
                }
            }
        }
    }
}
