using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace SudokuMaster.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessFile(string FileName)
        {

            string Result = "";
            ResultProcessor c_processor = new ResultProcessor();
            Result = c_processor.FillTheBoard(FileName);

        return Json(Result);
        }

        [HttpPost]
        public ActionResult SolveThePuzzle(string FileName)
        {

            List<string> Result = new List<string>();
            ResultProcessor c_processor = new ResultProcessor();
            Result = c_processor.ReadingFile(FileName);
            return Json(Result);
        }
    }
}
