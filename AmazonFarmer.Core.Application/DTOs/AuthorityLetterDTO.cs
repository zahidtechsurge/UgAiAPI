﻿using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Authority Letter List
    /// </summary>
    public class AuthorityLetterList
    {
        public int AuthorityLetterID { get; set; } // Property for Authority Letter ID
        public string AuthorityLetterNo { get; set; } // Property for Authority Letter number
        public string LetterDate { get; set; } // Property for letter date
        public string Product { get; set; } // Property for product
        public string Dealer { get; set; } // Property for dealer
        public int Quantity { get; set; } // Property for quantity
        public string Bearer { get; set; } // Property for bearer
        public string BearerNic { get; set; } // Property for bearer's NIC (National Identity Card)
        public bool AutoGenerated { get; set; } // Property for auto-generated status
        public string InvoiceNo { get; set; } // Property for invoice number
        public string Status { get; set; } // Property for status
    }
    public class add_AuthorityLetter_Res
    {
        public Int64 orderID { get; set; } = 0;
        public int productID { get; set; } = 0;
        public int qty { get; set; } = 0;
        public string biltyNo { get; set; } = string.Empty;
        public string truckerNo { get; set; } = string.Empty;
        public string brearName { get; set; } = string.Empty;
        public string bearerNIC { get; set; } = string.Empty;
        public string fieldWHIncharge { get; set; } = string.Empty;
    }
    public class update_AuthorityLetter_Req
    {
        public int authorityLetterID { get; set; } = 0;
        public bool isOCRAutomated { get; set; } = false;
        public int attachmentID { get; set; } = 0;
        public string? invoiceNumber { get; set; }
        public string? cnicNumber { get; set; }
    }
    public class authorityLetter_Invoice
    {
        public string sapOrderID { get; set; } = string.Empty;
        public string sapFarmerCode { get; set; } = string.Empty;
        public string qty { get; set; } = string.Empty;
        public string invoiceNumber { get; set; } = string.Empty;
        public string invoiceDate { get; set; } = string.Empty;
        public string invoiceAmount { get; set; } = string.Empty;
    }
    public class authorityLetter_GetINvoices_Req
    {
        public int letterID { get; set; }
        public string sapOrderID { get; set; }
    }
    public class authorityLetter_GetDetails
    {
        public int letterID { get; set; }
    }
    public class authorityLetter_GetOrderDetail_Resp
    {
        public Int64 orderID { get; set; }
        public DateTime orderDate { get; set; }
        public string warehouseInchargeName { get; set; } = string.Empty;
        public List<authorityLetter_GetOrderDetail_Product> products { get; set; } = new();
    }
    public class authorityLetter_GetOrderDetail_Product
    {
        public int productID { get; set; }
        public string productCode { get; set; } = string.Empty;
        public string productImage { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public int qty { get; set; }
    }
    public class authorityLetter_GetDetails_Resp
    {
        public int letterID { get; set; }
        public string letterNo { get; set; }
        public DateTime letterCreationDate { get; set; }
        public string orderNo { get; set; }
        public string sapOrderID { get; set; }
        public DateTime? orderDate { get; set; }
        public string bearerName { get; set; }
        public string bearerNIC { get; set; }
        public string godownIncharge { get; set; }
        public string invoiceNo { get; set; }
        public string pdfGUID { get; set; }
        public uploadAttachmentResp attachment { get; set; }
        public List<authorityLetter_Product_Resp> products { get; set; }
        public bool canEdit { get; set; }
    }
    public class authorityLetter_Product_Resp
    {
        public int productID { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public int productQTY { get; set; }
        public string warehouseName { get; set; }
        public string biltyNo { get; set; }
        public string truckerNo { get; set; }
    }
    public class getAuthorityLetters_Req
    {
        public Int64 orderID { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public string search { get; set; }
    }
    public class getAuthorityLettersEmployee_Req
    {
        public int skip { get; set; }
        public int take { get; set; }
        public string search { get; set; }
    }
    public class getAuthorityLetters_Resp
    {
        public int TotalQTY { get; set; } = 0;
        public int PendingQTY { get; set; } = 0;
        public pagination_Resp pagination { get; set; } = new pagination_Resp();
    }
    public class getAuthorityLetters_RespList
    {
        public int letterID { get; set; }
        public string letterNo { get; set; }
        public DateTime creationDate { get; set; }
        public string product { get; set; }
        public string farmerName { get; set; }
        public string qty { get; set; }
        public string bearerName { get; set; }
        public string bearerNIC { get; set; }
        public string warehouseInchage { get; set; }
        public bool autoGenerated { get; set; }
        public string invoiceNumber { get; set; }
        public string status { get; set; }
        public bool isEditable { get; set; }
        public bool canDelete { get; set; }
        public bool isDeleted { get; set; }
    }
    public class validateCNIC_Req
    {
        public int letterID { get; set; }
        public string name { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
    }
    public class validateCNIC_Resp
    {
        public int attachmentID { get; set; }
        public string cnicNumber { get; set; }
    }
    public class removeAuthorityLetterReq
    {
        public int authorityLetterID { get; set; }
    }


}
