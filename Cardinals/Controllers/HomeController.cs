using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Astros.Models;
using System.Data.EntityClient;
using System.Configuration;

namespace GroceryList.Controllers
{
    public class HomeController : Controller
    {

        public cardsEntities ge = new cardsEntities();

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPlayers()
        {
            var items = (from p in ge.players
                         join ps in ge.player_stats on p.player_id equals ps.player_id
                         orderby p.player_name
                         select new
                         {
                             player_id = p.player_id,
                             name = p.player_name
                         }).ToList().Distinct();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPlayerDataByCount(int q)
        {
            var items = (from p in ge.player_stats
                         orderby p.count_balls, p.count_strikes, p.pitch_type
                         where p.player_id == q && p.bat_side == "B" && p.count_balls < 9 && p.count_strikes < 9
                         select new
                         {
                             count_balls = p.count_balls,
                             count_strikes = p.count_strikes,
                             num_balls = p.num_balls,
                             num_strikes = p.num_strikes,
                             num_pitches = p.num_pitches,
                             avg_speed = p.avg_speed,
                             avg_horz = p.avg_horz,
                             avg_vert = p.avg_vert,
                             pitch_type = p.pitch_type
                         }).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPlayerDataOverall(int q)
        {
            var items = (from p in ge.player_stats
                         where p.player_id == q && p.bat_side == "B" && p.count_strikes == 9 && p.count_balls == 9
                         orderby p.pitch_type
                         select new
                         {
                             num_balls = p.num_balls,
                             num_strikes = p.num_strikes,
                             num_pitches = p.num_pitches,
                             avg_speed = p.avg_speed,
                             avg_horz = p.avg_horz,
                             avg_vert = p.avg_vert,
                             pitch_type = p.pitch_type
                         }).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPlayerDataSplit(int q)
        {
            var left = (from p in ge.player_stats
                         where p.player_id == q && p.bat_side == "L" && p.count_strikes == 9 && p.count_balls == 9
                         orderby p.pitch_type
                         select new
                         {
                             num_balls = p.num_balls,
                             num_strikes = p.num_strikes,
                             num_pitches = p.num_pitches,
                             avg_speed = p.avg_speed,
                             avg_horz = p.avg_horz,
                             avg_vert = p.avg_vert,
                             pitch_type = p.pitch_type
                         }).ToList();

            var right = (from p in ge.player_stats
                        where p.player_id == q && p.bat_side == "R" && p.count_strikes == 9 && p.count_balls == 9
                        orderby p.pitch_type
                        select new
                        {
                            num_balls = p.num_balls,
                            num_strikes = p.num_strikes,
                            num_pitches = p.num_pitches,
                            avg_speed = p.avg_speed,
                            avg_horz = p.avg_horz,
                            avg_vert = p.avg_vert,
                            pitch_type = p.pitch_type
                        }).ToList();

            return Json(new { right = right, left = left }, JsonRequestBehavior.AllowGet);
        }

    }
}
