using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AmazonFarmerAPI.Extensions
{
    public class PDFExtension
    {
        private readonly IConverter _converter;

        public PDFExtension(IConverter converter)
        {
            _converter = converter;
        }


        public async Task<string> generateSalesTaxCertificatePDF(InvoiceDTO DTO, ProductSalesOrgInfo belonging)
        {
            string resp = string.Empty;

            #region pdfString
            string pdfString = @"<!DOCTYPE html>
                <html>
                <head>
                <title>Page Title</title>
                </head>
                <style>
                table{border-collapse: collapse;}
                td, th{border: 1px solid gray;font-size:8px;padding:3px;}
                strong{font-size:20px;font-family:arial}
                tfooter tr td{font-weight:bold;}
                hr,img {margin:0;padding:0;}
                </style>
                <body>
                <div style=""text-align:right;font-size:8px"">
                    <u>Customer Copy</u>
                </div>
                <table style=""border:none;width:100%;margin:0;"">
                	<tbody>
                		<tr>
                      		<td style=""border:none;font-weight:bold;bottom:0;font-size:11px;"">
                            " + (belonging.Name) + @"
                            <br/>
                            " + (belonging.Address) + @"
                            <br/>
                            Tel: " + DTO.phoneNumber + @"
                            <br/>
                            NTN:" + DTO.nTNDealer + @"
                            <br/>
                            STRN:" + DTO.salesTaxNumberDealer + @"
                            <br/>
                            </td>
                            <td style=""border:none;text-align:right;vertical-align: middle;"">
                            	<img style=""width:150px!important"" src=" + belonging.LogoBase + @"/>
                            </td>
                        </tr>
                	</tbody>
                </table>
                <hr/>
                <br/>
                <div style=""text-align:center;""><strong>SALES TAX INVOICE</strong></div>
                <br/>
                <table style=""width:100%;"">
                <tbody>
	                <tr>
    	                <td style=""font-weight:bold;"">Dealer Code</td>
    	                <td>" + (DTO.dealerCode) + @"</td>
    	                <td style=""font-weight:bold;"">Sale Order Number</td>
    	                <td>" + (DTO.salesOrderNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Dealer Name</td>
    	                <td>" + (DTO.dealerName) + @"</td>
    	                <td style=""font-weight:bold;"">Sale Order Date</td>
    	                <td>" + (DTO.salesOrderDate) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Ship To Party-Code</td>
    	                <td>" + (DTO.shiptopartycode) + @"</td>
    	                <td style=""font-weight:bold;"">Delivery Number</td>
    	                <td>" + (DTO.deliveryNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Ship To Party-Name</td>
    	                <td>" + (DTO.shiptopartyName) + @"</td>
    	                <td style=""font-weight:bold;"">Shipment Number</td>
    	                <td>" + (DTO.shipmentNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Payer</td>
    	                <td>" + (DTO.payer) + @"</td>
    	                <td style=""font-weight:bold;"">Shipment Date</td>
    	                <td>" + (DTO.shipmentDate) + @"</td>
                    </tr>
	                <tr>
    	                <td rowspan=""4"" style=""font-weight:bold;"">Ship To Party-Address</td>
    	                <td rowspan=""4"">" + (BreakStringAfterWords(DTO.shiptopartyAddress, 7)) + @"</td>
    	                <td style=""font-weight:bold;"">Delivery Mode</td>
    	                <td>" + (DTO.deliveryMode) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Supply Source Code</td>
    	                <td>" + (DTO.supplySourceCode) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Supply Source</td>
    	                <td>" + (DTO.supplySourceDescription) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Sales Point</td>
    	                <td>" + (DTO.salesPoint) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Phone Number</td>
    	                <td>" + (DTO.phoneNumber) + @"</td>
    	                <td style=""font-weight:bold;"">Invoice Number</td>
    	                <td>" + (DTO.invoiceNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Sales Area</td>
    	                <td>" + (DTO.salesArea) + @"</td>
    	                <td style=""font-weight:bold;"">Invoice Date</td>
    	                <td>" + (DTO.invoiceDate) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Sales Tax Number-Dealer</td>
    	                <td>" + (DTO.salesTaxNumberDealer) + @"</td>
    	                <td style=""font-weight:bold;"">Truck Number</td>
    	                <td>" + (DTO.truckNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">NIC Number-Dealer</td>
    	                <td>" + (DTO.cNICNumberDealer) + @"</td>
    	                <td style=""font-weight:bold;"">Forward Agent Name</td>
    	                <td>" + (DTO.forwardAgentName) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">NTN-Dealer</td>
    	                <td>" + (DTO.nTNDealer) + @"</td>
    	                <td style=""font-weight:bold;"">Payment Due Date</td>
    	                <td>" + (DTO.paymentDueDate) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Filer/Non-Filer</td>
    	                <td>" + (DTO.filerNonFilerStatus) + @"</td>
    	                <td style=""font-weight:bold;"">BG Number</td>
    	                <td>" + (DTO.bGNumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Loading Dock</td>
    	                <td>" + (DTO.loadingDock) + @"</td>
    	                <td style=""font-weight:bold;"">Distance (KM)</td>
    	                <td>" + (DTO.distance) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Issue Plant</td>
    	                <td>" + (DTO.issuingPlant) + @"</td>
    	                <td style=""font-weight:bold;"">PSA Number</td>
    	                <td>" + (DTO.pSANumber) + @"</td>
                    </tr>
	                <tr>
    	                <td style=""font-weight:bold;"">Second Truck Number</td>
    	                <td>" + (DTO.vessel) + @"</td>
                    </tr>
                </tbody>
                </table>

                <br/>

                <table style=""width:100%;"">
	                <thead>
    	                <tr>
        	                <th>Material Code</th>        	
                            <th>Material Description</th>        	
                            <th>HS Code</th>        	
                            <th>Batch Number</th>        	
                            <th>QTY (Bags)</th>        	
                            <th>Rate</th>        	
                            <th>Gross Price</th>        	
                            <th>Discount</th>        	
                            <th>Price Excluding GST</th>        	
                            <th>Sales Tax</th>        	
                            <th>Price Including GST</th>

    	                </tr>
	                </thead>
	                <tbody>
                        <tr>
                            <td style=""text-align:center;"">" + (DTO.materialCode) + @"</td>
                            <td style=""text-align:center;"">" + (DTO.materialDescription) + @"</td>
                            <td>" + (DTO.hSCode) + @"</td>
                            <td style=""text-align:center;"">" + (DTO.batchNumber) + @"</td>
                            <td style=""text-align:center;"">" + (DTO.quantity) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.invoicingRate) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.grossPrice) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.discount) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.priceExcludingGST) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.salesTax) + @"</td>
                            <td style=""text-align:right;"">" + (DTO.priceIncludingGST) + @"</td>
    	                </tr>
                    </tbody>
	                <tfoot>
    	                <tr>
                        <td colspan=""3"" style=""font-weight:bold;"">Total</td>
                        <td></td>
                        <td style=""font-weight:bold;text-align:center;"">" + (DTO.quantity) + @"</td>
                        <td></td>
                        <td style=""font-weight:bold;text-align:right;"">" + (DTO.grossPrice) + @"</td>
                        <td style=""font-weight:bold;text-align:right;"">" + (DTO.discount) + @"</td>
                        <td style=""font-weight:bold;text-align:right;"">" + (DTO.priceExcludingGST) + @"</td>
                        <td style=""font-weight:bold;text-align:right;"">" + (DTO.salesTax) + @"</td>
                        <td style=""font-weight:bold;text-align:right;"">" + (DTO.priceIncludingGST) + @"</td>
    	                </tr>
                    </tfoot>
                </table>

                <br/>

				<table style=""border:none;width:100%;"">
                	<tbody>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">Price Excluding FED</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.priceExcludingFED) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">FED Amount</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.fEDAmount) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">Price Including FED</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.priceIncludingFED) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">Further Tax ( 0.00 % )</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.furtherTax) + @"</td>
                    	</tr>
                        <tr>
                        	<td style=""border:none;font-weight:bold;"">BG / Credit Charges</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.bGCreditCharges) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">Total Exclusing AIT</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.priceExcludingAIT) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">AIT (" + DTO.aITRate + @" %)</td>
                        	<td style=""border:none;text-align:right;font-weight:bold;"">" + (DTO.aIT) + @"</td>
                    	</tr>
                    	<tr>
                        	<td style=""border:none;font-weight:bold;"">Total Amount including AIT </td>
                        	<td style=""border-right:0px;border-left:0px;text-align:right;font-weight:bold;width:70px"">" + (DTO.priceIncludingAIT) + @"</td>
                    	</tr>
                	</tbody>
                </table>

                </body>
                </html>
                
                ";
            #endregion pdfString

            return await _htmlToPDF(resp, Path.Combine("private-documents", "temp-SalesTaxCertificate"), string.Concat("MaterialCode ", DTO.materialCode, " - SapOrderID ", DTO.sAPOrderID + "-" + DateTime.UtcNow.ToString("ddMMyyyy_hhmmff") + ".pdf"));
        }

        public async Task<string> generatePlanSummary(planSummary summary, EPlanSummaryType summaryType)
        {
            string resp = string.Empty;

            resp = $@"
               <!DOCTYPE html>
                <html lang=""en"">
                <head>
                  <meta charset=""UTF-8"">
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                  <title>Delivery Calendar</title>
                  <style>
                    body {{
                      font-family: sans-serif;
                      margin: 0;
                      padding: 0;
                      background-color:lightblue;
                    }}

                    #calendar {{
                      width: 80%; /* Adjust width based on image */
                      max-width: 1024px;
                      margin: 0 auto;
                      text-align: center;
                    }}

                    .calendar {{
                      border:2px solid #ccc;
                      padding: 20px;
                      background-color:#f5f5f5;
                    }}

                    .calendar th,
                    .calendar td {{
                      border: 1px solid red;
                      padding: 10px;
                      font-size:16px;
                      color: #333;
                    }}
                    table {{
                      width: 100%;
                      border: 1px solid black;
                      border-collapse: collapse; /* Ensures no gaps between borders */
                    }}
                    th,
                    td {{
                      border: 1px solid black;
                      padding: 10px;
                      text-align: left;
                    }}

                    th:first-child,
                    td:first-child {{
                      text-align: left;
                      font-weight: bold; /* Bold for headers */
                    }}

                    .calendar h1 {{
                      font-size:24px;
                      color: blue;
                    }}

                    .calendar p {{
                      font-size: 14px;
                      color:  #666;
                      /* Add other styles for location (e.g., margin-top: 10px) */
                    }}
                  </style>
                </head>
                <body>
                  <div id=""calendar"">
                    <h1 class=""calendar"">" + summary.season + @"</h1> 
                    " + await createTable(summary, summaryType) + @"
                    <p class=""calendar"">Warehouse Location:" + summary.warehouseLocation + @"</p> </div>
                </body>
                </html>";

            return await _htmlToPDF(resp, Path.Combine("private-documents", "temp-planSummary"), "planSummary-" + summary.season + "-" + summary.warehouseLocation + "-" + DateTime.UtcNow.ToString("ddMMyyyy_hhmmff") + ".pdf");
        }
        private async Task<string> createTable(planSummary summary, EPlanSummaryType summaryType)
        {
            string resp = string.Empty;
            string headerMonth = string.Empty;
            string body = string.Empty;
            if (summaryType == EPlanSummaryType.crop)
            {
                foreach (var item in summary.crops)
                {
                    string cropName = item.crops.Count() == 0 ? "" : item.crops.Count() > 1 ? (item.crops[0].cropName + " and " + (item.crops.Count() - 1) + " more") : item.crops[0].cropName;
                    body += @"<tr>
                                <td>" + cropName + @"<br/>" + item.acreage + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 10 || x.monthID == 4).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 11 || x.monthID == 5).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 12 || x.monthID == 6).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 1 || x.monthID == 7).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 2 || x.monthID == 8).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.products.Where(x => x.monthID == 3 || x.monthID == 9).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                          </tr>";
                }
            }
            else if (summaryType == EPlanSummaryType.product)
            {

                foreach (var item in summary.products)
                {
                    body += @"<tr>
                                <td>" + item.product + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 10 || x.monthID == 4).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 10 || x.monthID == 4).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 10 || x.monthID == 4).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 11 || x.monthID == 5).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 11 || x.monthID == 5).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 11 || x.monthID == 5).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 12 || x.monthID == 6).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 12 || x.monthID == 6).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 12 || x.monthID == 6).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 1 || x.monthID == 7).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 1 || x.monthID == 7).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 1 || x.monthID == 7).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 2 || x.monthID == 8).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 2 || x.monthID == 8).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 2 || x.monthID == 8).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                                <td>" + (item.months.Where(x => x.monthID == 3 || x.monthID == 9).FirstOrDefault() != null ? item.product : "") + " " + (item.months.Where(x => x.monthID == 3 || x.monthID == 9).FirstOrDefault()?.uom) + "<br>" + (item.months.Where(x => x.monthID == 3 || x.monthID == 9).FirstOrDefault()?.totalProducts.ToString("N2")) + @"</td>
                          </tr>";
                }
            }

            foreach (var item in summary.months)
            {
                headerMonth += @"<th>" + item.month + @"</th>";
            }
            resp += @"<table>
                        <thead>
                          <tr>
                            <th>" + summaryType.ToString() + @"</th>
                            " + headerMonth + @"
                          </tr>
                        </thead>
                        <tbody>
                            " + body + @"
                        </tbody>
                    </table>";
            return resp;
        }

        public byte[] generateAuthorityLetterBytes(TblAuthorityLetterDetails letterDetails, TblOrders order)
        {
            var prod = order.Products.FirstOrDefault().Product;
            string ProductCode = prod.ProductCode.PadLeft(10, '0');
            decimal? Mat = (letterDetails.BagQuantity * prod.UOM.UnitofMeasureReporting) / prod.UOM.UnitOfMeasureSales;
            string Detail = "<div class='m-t'>";
            //foreach (var item in input.tblAuthorityLetterDtls)
            //{
            Detail += "<p>Truck Number. <u>" + letterDetails.TruckerNo.Trim() + "</u> Bilty Number. <u>" + letterDetails.BiltyNo.Trim() + "</u> Bags Qty. <u>" + letterDetails.BagQuantity + "</u></p>";
            //}
            Detail += "</div>";
            StringBuilder Html = new StringBuilder();
            Html.Append(@"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta name='viewport' content='width=device-width' />
                                <title>AuthorityLetterFormat</title>
                            </head>
                            <body>
                                <style>
                                    p {
                                        font-size: 12px;
                                        font-family: sans-serif;
                                        word-spacing: 4px;  
                                        margin-top: -9px;
                                    }

                                    .m-t {
                                        margin-top: 21px;
                                    }
                                    u {
                                        
                                        text-decoration: none;
                                        bottom: 0px;
                                        border-width: 0 0 1px;
                                        border-style: solid;
                                    }

                                </style>
                                <div style='padding: 0px 40px 0 40px'>
                                    <div class='' style='text-align:center;'>
                                        <p style='font-weight: bold;'>Exhibit 3 D - 2</p>
                                        <p style='font-size: 16px;font-weight: bold;margin-top: -8px;'>ENGRO FERTILIZERS LIMITED</p>
                                        <p style='font-size: 14px;font-weight: 700;margin-top: -16px;'>Authority Letter 1</p>
                                    </div>


                                    <div class='m-t'>
                                        <p>Authority Letter # " + letterDetails.AuthorityLetters.AuthorityLetterNo.Trim() + @" </p>
                                        <p>Dated <u>" + DateTime.Now.ToString("dd/MM/yyyy").Trim() + @"</u></p>
                                        <p>The warehouse Incharge </p>
                                        <p>Engro Fertilizers Limited</p>
                                        <p>Godown <u>" + order.Warehouse.WHCode.Trim() + @"</u></p>
                                    </div>

                                    <div class='m-t'>
                                        <p>As-Salam-o-Alaikum</p>
                                        <p>The Bearer of this authority letter Mr. <u>" + letterDetails.AuthorityLetters.BearerName.Trim() + @"</u> Holding NIC. <u>" + letterDetails.AuthorityLetters.BearerNIC.Trim() + @"</u> is Authorized to sign the Invoice and uplift the product.</p>
                                       
                                    </div>

                                    " + Detail + @" 
                                    <div class='m-t'>
                                        <p>may please be loaded against our customer order number : <u>" + order.OrderID.ToString().Trim() + @"</u></p>
                                        <p>Dated  <u>" + order.CreatedOn.ToString("yyyy-MM-dd").Trim() + @"</u>  <br>For Material <u>" + prod.Name?.Trim() + @"</u> For <u>" + (Mat == null ? string.Empty : Mat) + @" Met</u> on the above-mentioned truck </p>
                                    </div>

                                    <div style='margin-top: 26px;'>
                                        <p>We shall be grateful to you for this.</p>
                                    </div>

                                    <div style=' margin: 21px 0 0 0px;'>
                                        <p>Thanking You,</p>
                                    </div>

                                    <div style=' margin: 21px 0 0 0px;'>
                                        <p>Verified By:</p>
                                    </div>
                                    
                                    <table style=' margin: 21px 0 0 0px;'>            
                                        <tr>
                                        <td style='width: 219px;'>
                                            <p>Dealer Code " + order.User.FarmerProfile.FirstOrDefault().SAPFarmerCode?.TrimStart('0') + @"</p>
                                            <p>Dealer Name " + order.User.FirstName?.TrimStart('0') + @"</p>
                                            <p>Ship To Party-Code " + order.User.ShipToPartyCode?.TrimStart('0') + @"</p>
                                            <p>Ship To Party-Name " + order.User.ShipToPartyName?.TrimStart('0') + @"</p>
                                        </td>
                                        <td style='width: 79px;'></td>
                                        <td>
                                            <p>Bearer's Signature </p><br />
                                            <p>Driver's Signature ________________________</p>
                                            <p>Driver's Name _____________________________</p>
                                            <p>S/O _______________________________________</p>
                                        </td>
                                        </tr>
                                    </table>

                                </div>
                            </body>
                            </html>

                            ");
            //return await _htmlToPDF(Html.ToString(), Path.Combine("private-documents", "AuthorityLetter-PDF"), "AuthorityLetter-" + order.OrderID + "-" + prod.ProductCode + "-" + DateTime.UtcNow.ToString("ddMMyyyy_hhmmff") + ".pdf");
            return ConvertHtmlToPdf(Html.ToString());
        }







        private string BreakStringAfterWords(string input, int wordsPerLine)
        {
            string[] words = input.Split(' ');
            StringBuilder output = new StringBuilder();
            int wordCount = 0;

            foreach (string word in words)
            {
                if (wordCount > 0 && wordCount % wordsPerLine == 0)
                {
                    output.Append("<br/>");
                }

                output.Append(word).Append(' ');
                wordCount++;
            }

            return output.ToString().Trim();
        }

        private async Task<string> _htmlToPDF(string html, string filePath, string fileName)
        {
            byte[] fileBytes = ConvertHtmlToPdf(html);
            // Write the image bytes to the file
            filePath = Path.Combine(filePath, fileName);
            System.IO.File.WriteAllBytes(filePath, fileBytes);
            return filePath;
        }
        private byte[] ConvertHtmlToPdf(string html)
        {
            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Landscape;
            globalSettings.PaperSize = PaperKind.A4;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            headerSettings.FontSize = 15;
            headerSettings.FontName = "Ariel";
            //headerSettings.Right = "Page [page] of [toPage]";
            headerSettings.Line = true;
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 12;
            footerSettings.FontName = "Ariel";
            //footerSettings.Center = "This is for demonstration purposes only.";
            footerSettings.Line = true;
            objectSettings.HeaderSettings = headerSettings;
            objectSettings.FooterSettings = footerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return _converter.Convert(htmlToPdfDocument);
        }

    }
}
