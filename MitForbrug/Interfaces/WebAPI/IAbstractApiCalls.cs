using System;

namespace MitForbrug.Interfaces.WebAPI
{
    public interface IAbstractApiCalls
    {
        string GetSignalsByMeterIdAndSignalType(int meterId, int signaltype);
        string GetJSONfromApiRequest(string urlPart, string body = "");

        string GetContractsByPersonGuid(string personGuid);
        string GetEarliestMeasurementBySignalId(int signalId, DateTime sinceDate);
        string GetLastMeasurementBySignalId(int signalId, DateTime sinceDate);
        string GetLeaseByNumber(int personNumberId);
        string GetLeaseInfoByAddress(string address);
        string GetLeaseInfoByGroupGUID(string groupGUID);
        string GetLeaseInfoByGroupNumbers(string companyNumber, string divisionNumber, string leaseNumber);
        string GetLoginByEmailAndPassword(string email, string password);
        string GetLoginByPersonIdAndPassword(int personId, string password);
        string GetMeasurementsBySignalId(int signalId, DateTime fromDate);
        string GetMeasurementsBySignalId(int signalId, DateTime fromDate, DateTime toDate);
        string GetMetersByLeaseGuid(string leaseGroupGUID);
        string GetMetersByLeaseGuidAndMediumId(string leaseGroupGUID, int deviceTypeId);
        string GetPersonsByNumber(int personNumber);
        string GetPrimaryTenantNumberByAdressAndOtherName(string adress, string othername);
        
        void SetCustomerInterfaceType(int cit);
        void SetCustomerInterfaceType(string cit);
        int CustomerInterfaceType { get; }
        void SetCustomerName(string customerName);
    }
}