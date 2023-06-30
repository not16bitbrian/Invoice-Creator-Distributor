# Invoice-Creator-Distributor

A console application in C# that reads the contents of file BillFile.xml, parses its contents and writes out a file in the format specified in doc BillsOutput.txt. 
Named the export file BillFile-mmddyyyy.rpt. The values for the fields in square brackets should either be populated from the file, a constant variable referenced 
in the table below, or be the result of a calculation from values in the file. Fields not existing in the file spec should be omitted from the output file.


FieldID	Value/Reference
2	8203ACC7-2094-43CC-8F7A-B8F19AA9BDA2
5	Count of IH records
6	SUM of BILL_AMOUNT values 
JJ	8E2FEA69-5D77-4D0F-898E-DFA25677D19E
OO	5 days after the current date
PP	3 days before the Due Date (MM)

*All dates should be in format MM/DD/YYYY
*Number fields do not require commas
*File Header record appears once per file
*AA record appears once per bill
*HH record appears once per bill


A routine in C# that reads the contents of BillFile.rpt and imports the data into the attached access database, Billing.mdb, into the corresponding tables and fields.

A routine in C# that connects to the “Billing” database, retrieves the contents of both tables and exports the records associated with an account in a CSV formatted file, 
outlined in the attached BillingReport.txt. File should include a header and one line per unique customer record and any bills associated to that customer.
