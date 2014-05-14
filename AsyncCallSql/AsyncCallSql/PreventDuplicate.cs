using System;
using System.Diagnostics;
using System.Globalization;

namespace AsyncCallSql
{
    public class PreventDuplicate
    {
        public Result PreventDuplicateTrx(TransactionRecord transactionRecord)
        {
            var result = new Result();
            
            var waiter = new Waiter(TimeSpan.FromSeconds(5));

            var requestMsg = Helper.Serialize1(transactionRecord);

            var hash = Helper.GenerateHash(requestMsg);

            var transactionId = transactionRecord.TransactionIdField;
            var accountNumber = transactionRecord.AccountNumberField;
            var amount = transactionRecord.TransactionAmountField;


            result.AdditionalDataField = "Se agotó tiempo de espera de respuesta del proveedor.";
            result.PrintOnReceiptField = "";
            result.ResultCodeField = "99";

            //Inserta si no existe el pago (RequestMessage)
            CheckPayment.AddPayment(accountNumber, amount, transactionId, DateTime.Now, requestMsg, hash, TransactionStatus.Status.Pending, 0);

            //Lee si existe el mensaje con el mismo numero de Trx y Hash + Status {0, 2}
            var retorno = CheckPayment.CheckPaymentExist(transactionId, hash);

            if (String.IsNullOrEmpty(retorno))
            {

                var intervalos = 0;


                while (intervalos != 11)
                {
                    retorno = CheckPayment.CheckPaymentExist(transactionId, hash);

                    intervalos++;

                    Debug.Print("Intervalos: " + intervalos.ToString());
                    Debug.Print("Valor de Retorno: " + retorno);

                    if (!String.IsNullOrEmpty(retorno))
                    {

                        Debug.Print("Retorno: " + retorno);

                        var responseMessage = Helper.Deserialize<Result>(retorno);

                        result.AdditionalDataField = responseMessage.AdditionalDataField;
                        result.ResultCodeField = responseMessage.ResultCodeField;
                        result.PrintOnReceiptField = responseMessage.PrintOnReceiptField;
                    }
                    else
                    {
                        waiter.Wait();

                        if (intervalos == 11)
                        {
                            UpdateResponseMessage(transactionId, result, TransactionStatus.Status.Error);
                        }

                        Debug.Print(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    }

                }

            }
            else{

                Debug.Print("Retorno: " + retorno);

                var responseMessage = Helper.Deserialize<Result>(retorno);

                result.AdditionalDataField = responseMessage.AdditionalDataField;
                result.ResultCodeField = responseMessage.ResultCodeField;
                result.PrintOnReceiptField = responseMessage.PrintOnReceiptField;

            }
            

            return result;
        }


        public void UpdateResponseMessage(string transactionId, Result result, TransactionStatus.Status transactionStatus)
        {

            var responseMsg = Helper.Serialize1(result);

            CheckPayment.UpdatePayment(transactionId, responseMsg, transactionStatus);

        }

    }
}