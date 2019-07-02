using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem {
	class ManagementService {
		// 得到所有运动员信息
		public static List<Athletes> GetAllAthletes() {
			using(var db = new ManagementDatabase()) {
				List<Athletes> athletes = db.Athletes.ToList();
				return athletes;
			}
		}

		// 查询运动员相关信息
		public static List<Athletes> SearchAthletes(string attribute, string s) {
			// 分门别类进行查询
			using(var db = new ManagementDatabase()) {
				if(attribute == "IDNumber") {
					return db.Athletes.Where(o => o.IDNumber.Contains(s)).ToList();
				}

				if(attribute == "Name") {
					return db.Athletes.Where(o => o.Name.Contains(s)).ToList();
				}

				if(attribute == "Age") {
					int age = Int32.Parse(s);
					return db.Athletes.Where(o => o.Age == age).ToList();
				}

				if(attribute == "Team") {
					return db.Athletes.Where(o => o.Team.Contains(s)).ToList();
				}

				if(attribute == "Event") {
					return db.Athletes.Where(o => o.Event.Contains(s)).ToList();
				}

				if(attribute == "AthleteNumber") {
					int athleteNumber = Int32.Parse(s);
					return db.Athletes.Where(o => o.AthleteNumber == athleteNumber).ToList();
				}

				return null;
			}
		}

		// 删除运动员
		public static void DeleteAthlete(string IDNumber) {
			using(var db = new ManagementDatabase()) {
				var athlete = db.Athletes.SingleOrDefault(o => o.IDNumber == IDNumber);
				db.Athletes.Remove(athlete ?? throw new InvalidOperationException());
				db.SaveChanges();
			}
		}

		// 修改运动员信息
		public static void ReviseAthlete(string IDNumber, string attribute, string text) {
			using(var db = new ManagementDatabase()) {
				var athlete = db.Athletes.SingleOrDefault(o => o.IDNumber == IDNumber);
				Debug.Assert(athlete != null, nameof(athlete) + " != null");
                switch (attribute) {
                    case "IDNumber":
	                    athlete.IDNumber = text;
						break;
                    case "Name":
	                    athlete.Name = text;
	                    break;
                    case "Sex":
	                    athlete.Sex = text;
	                    break;
                    case "Age":
	                    athlete.Age = Convert.ToInt32(text);
	                    break;
                    case "AthleteNumber":
	                    athlete.AthleteNumber = Convert.ToInt32(text);
	                    break;
                    case "Team":
	                    athlete.Team = text;
	                    break;
                    case "Event":
	                    athlete.Event = text;
	                    break;
                }
				db.SaveChanges();
            }
		}

		// 添加运动员信息
		public static void AddAthlete(Athletes athletes) {
			using(var db = new ManagementDatabase()) {
				db.Athletes.Add(athletes);
				db.SaveChanges();
			}
		}
	}
}