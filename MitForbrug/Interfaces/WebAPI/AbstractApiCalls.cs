using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitForbrug.Interfaces.WebAPI
{
    public abstract class AbstractApiCalls : IAbstractApiCalls
    {
        protected string customer;
        public int customerInterfaceType;

        public int CustomerInterfaceType
        {
            get
            {
                return customerInterfaceType;
            }
        }

        public void SetCustomerName(string customerName)
        {
            customer = customerName;
        }

        /// <summary>
        /// Sets the customer interface type
        /// 0 for egbolig
        /// 1 for ktp
        /// </summary>
        /// <param name="cit"></param>
        public void SetCustomerInterfaceType(int cit)
        {
            // 0 = egbolig
            // 1 = ktp
            customerInterfaceType = cit;
        }

        /// <summary>
        /// Sets the customer interface type
        /// "eg" for egbolig
        /// "ktp" for ktp
        /// </summary>
        /// <param name="cit"></param>
        public void SetCustomerInterfaceType(string cit)
        {
            switch (cit)
            {
                case "ktp":
                    customerInterfaceType = 1;
                    break;
                case "eg":
                    customerInterfaceType = 0;
                    break;
            }
        }

        /// <summary>
        /// 
        /// https://bovia.api.batechnic.dk/v1/Measurement?$filter=SignalID eq 583176 and MeasuredAt gt (datetime'2018-11-22T17:31:00')&$top=50&$orderby=MeasuredAt
        /// </summary>
        /// <param name="signalId"></param>
        /// <returns></returns>
        public string GetMeasurementsBySignalId(int signalId, DateTime fromDate)
        {
            var url = $"measurement?$filter=SignalID eq {signalId} and MeasuredAt gt (datetime'{fromDate.ToString("yyyy-MM-ddTHH:mm:ss")}')&$top=500&$orderby=MeasuredAt";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetEarliestMeasurementBySignalId(int signalId, DateTime sinceDate)
        {
            var url = $"measurement?$filter=SignalID eq {signalId} and MeasuredAt gt (datetime'{sinceDate.ToString("yyyy-MM-ddTHH:mm:ss")}')&$top=1&$orderby=MeasuredAt";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetLastMeasurementBySignalId(int signalId, DateTime sinceDate)
        {
            var url = $"measurement?$filter=SignalID eq {signalId} and MeasuredAt lt (datetime'{sinceDate.ToString("yyyy-MM-ddTHH:mm:ss")}')&$top=1&$orderby=MeasuredAt desc";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetMeasurementsBySignalId(int signalId, DateTime fromDate, DateTime toDate)
        {
            var url = $"measurement?$filter=SignalID eq {signalId} and MeasuredAt gt (datetime'{fromDate.ToString("yyyy-MM-ddTHH:mm:ss")}') and MeasuredAt lt (datetime'{toDate.ToString("yyyy-MM-ddTHH:mm:ss")}')&$top=500&$orderby=MeasuredAt";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        /// <summary>
        /// https://alabu.api.batechnic.dk/v1/Signal?$filter=MeterID eq 12141
        /// 
        /// </summary>
        /// <param name="signalId"></param>
        /// <returns></returns>
        public string GetSignalsByMeterIdAndSignalType(int meterId, int signaltype)
        {
            var partUrl = $"Signal?$filter=MeterID eq {meterId} and SignalType  eq '{signaltype}'";
            var jsonResponse = GetJSONfromApiRequest(partUrl);
            return jsonResponse;
        }

        /// <summary>
        /// 
        /// https://kunde.api.batechnic.dk/v1/GroupAndAttributeEnriched?groupGuid={404b3915-dd21-4a56-a659-fe57b56663d4}
        /// </summary>
        /// <param name="groupGUID"></param>
        /// <returns></returns>
        public string GetLeaseInfoByGroupGUID(string groupGUID)
        {
            var url = "GroupAndAttributeEnriched?groupGuid={" + groupGUID + "}";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetLeaseInfoByAddress(string address)
        {
            var url = $"GroupAndAttributeEnriched?address={address}";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetLeaseInfoByGroupNumbers(string companyNumber, string divisionNumber, string leaseNumber)
        {
            var resourceAndFilter = $"GroupAndAttributeEnriched?companyNumber={companyNumber}&divisionNumber={divisionNumber}&leaseNumber={leaseNumber}&strict=1";
            var jsonResponse = GetJSONfromApiRequest(resourceAndFilter);
            return jsonResponse;
        }

        public string GetMetersByLeaseGuid(string leaseGroupGUID)
        {
            var url = $"meter?$filter=GroupGUID eq guid'{leaseGroupGUID}'";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetMetersByLeaseGuidAndMediumId(string leaseGroupGUID, int deviceTypeId)
        {
            var url = $"meter?$filter=GroupGUID eq guid'{leaseGroupGUID}' and DeviceTypeID eq {deviceTypeId}";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public string GetPrimaryTenantNumberByAdressAndOtherName(string adress, string othername)
        {
            var url = $"person?$filter=Name2 eq '{othername}' and CurrentAddress eq '{adress}'";
            var jsonResponse = GetJSONfromApiRequest(url);
            return jsonResponse;
        }

        public abstract string GetJSONfromApiRequest(string urlPart, string body = "");

        public abstract string GetLeaseByNumber(int personNumberId);

        public abstract string GetContractsByPersonGuid(string personGuid);

        public abstract string GetLoginByPersonIdAndPassword(int personId, string password);

        public abstract string GetLoginByEmailAndPassword(string email, string password);

        public abstract string GetPersonsByNumber(int personNumber);
    }
}
