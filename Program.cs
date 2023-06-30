using System;
using System.Data.Odbc;

namespace BillImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            string rptFilePath = "BillFile-06272023.rpt";
            string mdbFilePath = "C:/Users/btj288/Downloads/ICAssignment/ICAssignment/Billing.mdb";

            string connectionString = $"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Dbq={mdbFilePath};";
            OdbcConnection connection = new OdbcConnection(connectionString);
            connection.Open();

            // Parse and import data from the .rpt file
            ParseAndImportBillFile(rptFilePath, connection);

            connection.Close();

            Console.WriteLine("Data import completed.");
            Console.ReadLine();
        }

        static void ParseAndImportBillFile(string rptFilePath, OdbcConnection connection)
        {
            string[] lines = System.IO.File.ReadAllLines(rptFilePath);

            // Start from line 1 to skip the header
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('|');

                // Check if the line contains the necessary data fields
                if (fields.Length >= 14)
                {
                    string customerName = GetValue(fields[2]);
                    string accountNumber = GetValue(fields[3]);
                    string customerAddress = GetValue(fields[4]);
                    string customerCity = GetValue(fields[5]);
                    string customerState = GetValue(fields[6]);
                    string customerZip = GetValue(fields[7]);

                    string billDate = GetValue(fields[9]);
                    string billNumber = GetValue(fields[10]);
                    string billAmount = GetValue(fields[11]);
                    string dueDate = GetValue(fields[12]);
                    string serviceAddress = GetValue(fields[13]);

                    // Insert data into the Customer table
                    OdbcCommand customerCommand = new OdbcCommand(
                        "INSERT INTO Customer (CustomerName, AccountNumber, CustomerAddress, CustomerCity, CustomerState, CustomerZip, DateAdded) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?)",
                        connection);
                    customerCommand.Parameters.AddWithValue("@CustomerName", customerName);
                    customerCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    customerCommand.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                    customerCommand.Parameters.AddWithValue("@CustomerCity", customerCity);
                    customerCommand.Parameters.AddWithValue("@CustomerState", customerState);
                    customerCommand.Parameters.AddWithValue("@CustomerZip", customerZip);
                    customerCommand.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    customerCommand.ExecuteNonQuery();

                    // Get the generated Customer ID
                    OdbcCommand customerIdCommand = new OdbcCommand("SELECT @@IDENTITY", connection);
                    int customerId = Convert.ToInt32(customerIdCommand.ExecuteScalar());

                    // Insert data into the Bills table
                    OdbcCommand billsCommand = new OdbcCommand(
                        "INSERT INTO Bills (BillDate, BillNumber, BillAmount, FormatGUID, AccountBalance, DueDate, ServiceAddress, FirstEmailDate, SecondEmailDate, DateAdded, CustomerID) " +
                        "VALUES (?, ?, ?, NULL, NULL, ?, ?, NULL, NULL, ?, ?)",
                        connection);
                    billsCommand.Parameters.AddWithValue("@BillDate", billDate);
                    billsCommand.Parameters.AddWithValue("@BillNumber", billNumber);
                    billsCommand.Parameters.AddWithValue("@BillAmount", billAmount);
                    billsCommand.Parameters.AddWithValue("@DueDate", dueDate);
                    billsCommand.Parameters.AddWithValue("@ServiceAddress", serviceAddress);
                    billsCommand.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    billsCommand.Parameters.AddWithValue("@CustomerID", customerId);
                    billsCommand.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine($"Skipping line {i + 1} due to insufficient data fields.");
                }
            }
        }

        static string GetValue(string field)
        {
            return field.Split('~')[1];
        }
    }
}
