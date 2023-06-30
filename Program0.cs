using System;
using System.IO;
using System.Xml.Linq;

namespace BillParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "BillFile.xml";
            string outputFile = $"BillFile-{DateTime.Now:MMddyyyy}.rpt";

            try
            {
                XDocument doc = XDocument.Load(inputFile);
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    WriteFileHeader(writer);
                    foreach (var billHeader in doc.Descendants("BILL_HEADER"))
                    {
                        WriteBillData(writer, billHeader);
                    }
                }

                Console.WriteLine($"File generated successfully: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadLine();
        }

        static void WriteFileHeader(StreamWriter writer)
        {
            // Get the current date in MM/DD/YYYY format
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");

            writer.WriteLine($"1~FR|2~{GetValue(2)}|3~Sample UT file|4~{currentDate}|5~{GetValue(5)}|6~{GetValue(6)}");
        }

        static void WriteBillData(StreamWriter writer, XElement billHeader)
        {
            string accountNumber = GetElementValue(billHeader, "Account_No");
            string customerName = GetElementValue(billHeader, "Customer_Name");
            string address1 = GetElementValue(billHeader, "Address_Information|Mailing_Address_1");
            string address2 = GetElementValue(billHeader, "Address_Information|Mailing_Address_2");
            string city = GetElementValue(billHeader, "Address_Information|City");
            string state = GetElementValue(billHeader, "Address_Information|State");
            string zip = GetElementValue(billHeader, "Address_Information|Zip");
            string invoiceNumber = GetElementValue(billHeader, "Invoice_No");
            string billDate = GetElementValue(billHeader, "Bill_Dt");
            string dueDate = GetElementValue(billHeader, "Due_Dt");
            string billAmount = GetElementValue(billHeader, "Bill|Bill_Amount");
            string balanceDue = GetElementValue(billHeader, "Bill|Balance_Due");
            string firstNotificationDate = GetDateOffset(5);
            string secondNotificationDate = GetDateOffset(-3);

            writer.WriteLine($"AA~CT|BB~{accountNumber}|VV~{customerName}|CC~{address1}|DD~{address2}|EE~{city}|FF~{state}|GG~{zip}");
            writer.WriteLine($"HH~IH|II~R|JJ~{GetValue("JJ")}|KK~{invoiceNumber}|LL~{billDate}|MM~{dueDate}|NN~{billAmount}|OO~{firstNotificationDate}|PP~{secondNotificationDate}|QQ~{balanceDue}|RR~{DateTime.Now:MM/dd/yyyy}|SS~{address1}");
        }

        static string GetElementValue(XElement element, string path)
        {
            var paths = path.Split('|');
            XElement targetElement = element;

            foreach (var pathSegment in paths)
            {
                if (targetElement == null)
                    return string.Empty;
                else
                    targetElement = targetElement.Element(pathSegment);
            }

            return targetElement.Value;
        }

        static string GetValue(int fieldId)
        {
            switch (fieldId)
            {
                case 2:
                    return "8203ACC7-2094-43CC-8F7A-B8F19AA9BDA2";
                case 5:
                    return "Count of IH records";
                case 6:
                    return "SUM of BILL_AMOUNT values";
                default:
                    return string.Empty;
            }
        }

        static string GetValue(string fieldId)
        {
            switch (fieldId)
            {
                case "JJ":
                    return "8E2FEA69-5D77-4D0F-898E-DFA25677D19E";
                default:
                    return string.Empty;
            }
        }

        static string GetDateOffset(int days)
        {
            DateTime targetDate = DateTime.Now.AddDays(days);
            return targetDate.ToString("MM/dd/yyyy");
        }
    }
}
