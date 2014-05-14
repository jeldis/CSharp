using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace AsyncCallSql
{
    public class CheckPayment
    {


        static readonly string ConnString = ConfigurationManager.ConnectionStrings["ePagoBtsActivitySupport"].ConnectionString;

        public static void AddPayment(string accountNumber, decimal amount, string paymentGuid,
            DateTime paymentDate, string originalMessage, string originalMessageHash,
            TransactionStatus.Status transactionStatus, int providerId)
        {

            try
            {

                using (var conn = new SqlConnection(ConnString))
                {
                    using (var comm = conn.CreateCommand())
                    {

                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "usp_AddPaymentMessageRequest";

                        var p1 = new SqlParameter
                            {
                                ParameterName = "@AccountNumber",
                                SqlDbType = SqlDbType.VarChar,
                                Size = 50,
                                Value = accountNumber
                            };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@Amount",
                            SqlDbType = SqlDbType.Decimal,
                            Value = amount
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@PaymentGuid",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = paymentGuid
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@PaymentDate",
                            SqlDbType = SqlDbType.DateTime,
                            Value = paymentDate
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@OriginalMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = originalMessage
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@OriginalMessageHash",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = originalMessageHash
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@TransactionStatus",
                            SqlDbType = SqlDbType.Int,
                            Value = transactionStatus
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@ProviderId",
                            SqlDbType = SqlDbType.Int,
                            Value = providerId
                        };

                        comm.Parameters.Add(p1);

                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        Debug.Print("Call to SQL AddPayment");

                        comm.ExecuteReader();

                    }
                }

            }
            catch (SqlException sqlException)
            {
                var ex = sqlException.ToString();
            }
            catch (Exception exception)
            {
                var ex = exception.ToString();
            }

        }


        public static string CheckPaymentExist(string transactionId, string originalMessageHash)
        {

            var returnValue = String.Empty;

            try
            {

                using (var conn = new SqlConnection(ConnString))
                {
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "usp_CheckPaymentMessageRequest";

                        var p1 = new SqlParameter
                        {
                            ParameterName = "@PaymentGuid",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = transactionId
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@OriginalMessageHash",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 64,
                            Value = originalMessageHash
                        };

                        comm.Parameters.Add(p1);


                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        Debug.Print("Call to SQL Select CheckPaymentExist");

                        using (var reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                returnValue = reader["PaymentReturnMessage"].ToString();
                                Debug.Print("Valor de retorno: " + returnValue);
                            }

                        }

                    }
                }
            }
            catch (SqlException sqlException)
            {
                var ex = sqlException.ToString();
            }
            catch (Exception exception)
            {
                var ex = exception.ToString();
            }


            return returnValue;
        }

        public static string UpdatePayment(string transactionId, string returnMessage, TransactionStatus.Status transactionStatus)
        {

            var returnValue = String.Empty;

            try
            {

                using (var conn = new SqlConnection(ConnString))
                {
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "usp_UpdatePaymentMessageResponse";

                        var p1 = new SqlParameter
                        {
                            ParameterName = "@PaymentGuid",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = transactionId
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@PaymentReturnMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = returnMessage
                        };

                        comm.Parameters.Add(p1);

                        p1 = new SqlParameter
                        {
                            ParameterName = "@TransactionStatus",
                            SqlDbType = SqlDbType.Int,
                            Value = transactionStatus
                        };

                        comm.Parameters.Add(p1);


                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        Debug.Print("Call to SQL Select UpdatePayment");

                        comm.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException sqlException)
            {
                var ex = sqlException.ToString();
            }
            catch (Exception exception)
            {
                var ex = exception.ToString();
            }


            return returnValue;
        }

    }
}