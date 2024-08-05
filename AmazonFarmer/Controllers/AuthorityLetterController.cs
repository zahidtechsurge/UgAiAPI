using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Controllers
{
    public class AuthorityLetterController : BaseController
    {
        private IRepositoryWrapper _repoWrapper;
        public AuthorityLetterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public IActionResult Index(int OrderID)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> getAuthorityLetters(int OrderID)
        {
            // Initialization.
            int start = 0, length = 0, draw = 0, recordsFiltered = 0, recordsTotal = 0;
            string order, orderDir, dir, sortColumn, search = "";

            List<AuthorityLetterList> AuthorityLetterList = new List<AuthorityLetterList>();

            try
            {
                //Assigning values
                start = Convert.ToInt32(Request.Form["start"].ToString());
                length = Convert.ToInt32(Request.Form["length"].ToString());
                search = Request.Form["search[value]"].ToString();
                order = Request.Form["order[0][column]"];
                orderDir = Request.Form["order[0][dir]"][0];
                dir = orderDir == "asc" ? "ascending" : "descending";
                sortColumn = Request.Form["columns[" + order + "][data]"][0] + " " + dir;
                draw = Convert.ToInt32(Request.Form["draw"].ToString());

                //DataSet
                IQueryable<TblAuthorityLetters> tblAuthorityLetters = await _repoWrapper.AuthorityLetterRepo.GetAuthorityLetterLists();

                //filter for OrderID
                tblAuthorityLetters = tblAuthorityLetters.Where(x => x.OrderID == OrderID);

                //Searching
                if (!string.IsNullOrEmpty(search))
                {
                    tblAuthorityLetters = tblAuthorityLetters.Where(x =>
                        x.AuthorityLetterNo.Contains(search) ||
                        x.Dated.Contains(search) ||
                        x.AuthorityLetterDetails.FirstOrDefault().Products.Name.Contains(search) ||
                        x.BearerName.Contains(search) ||
                        x.INVNumber.Contains(search) 
                    );
                }

                //Sorting
                if (order == "0")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.AuthorityLetterNo);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.AuthorityLetterNo);
                    }
                }
                else if (order == "1")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.Dated);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.Dated);
                    }
                }
                else if (order == "2")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.AuthorityLetterDetails.FirstOrDefault().Products.Name);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.AuthorityLetterDetails.FirstOrDefault().Products.Name);
                    }
                }
                else if (order == "3")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.DealerCode);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.DealerCode);
                    }
                }
                else if (order == "4")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.AuthorityLetterDetails.FirstOrDefault().BagQuantity);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.AuthorityLetterDetails.FirstOrDefault().BagQuantity);
                    }
                }
                else if (order == "5")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.BearerName);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.BearerName);
                    }
                }
                else if (order == "6")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.BearerNIC);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.BearerNIC);
                    }
                }
                else if (order == "8")
                {
                    if (orderDir == "asc")
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderBy(x => x.INVNumber);
                    }
                    else
                    {
                        tblAuthorityLetters = tblAuthorityLetters.OrderByDescending(x => x.INVNumber);
                    }
                }

                //mapping DTO
                AuthorityLetterList = tblAuthorityLetters.Select(x => new AuthorityLetterList
                {
                    AuthorityLetterID = x.AuthorityLetterID,
                    AuthorityLetterNo = x.AuthorityLetterNo,
                    AutoGenerated = x.IsOCRAutomated,
                    Bearer = x.BearerNIC,
                    BearerNic = x.BearerNIC,
                    Dealer = x.DealerCode,
                    InvoiceNo = x.INVNumber,
                    LetterDate = x.Dated,
                    Status = x.Status ? "Approved" : "Declined",
                    Product = x.AuthorityLetterDetails.FirstOrDefault().Products.Name,
                    Quantity = x.AuthorityLetterDetails.FirstOrDefault().BagQuantity
                }).ToList();

                recordsTotal = AuthorityLetterList.Count;
                recordsFiltered = AuthorityLetterList.Count;
            }
            catch (Exception)
            { }

            return Json(new
            {
                draw = Convert.ToInt32(draw + 1),
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = AuthorityLetterList.ToList()
            });
        }
    }
}
