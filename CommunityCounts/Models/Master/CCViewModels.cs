using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityCounts.Models.Master
{
    public class clientcaseWorkDetailList
    {
        public DateTime CaseServiceDate { get; set; }
        public DateTime CaseServiceEditDate { get; set; }
        public string name { get; set; }
        public string CaseServiceTime { get; set; }
        public string CaseServiceNotes { get; set; }
        public int idClientCaseServiceDetail { get; set; }
    }
    public class clientCaseWorkOverView
    {
        public DateTime CaseServiceDate { get; set; }
        public DateTime CaseServiceEditDate { get; set; }
        public string ServiceName { get; set; }
        public string Email { get; set; }
        public string CaseServiceNotes { get; set; }
        public string CaseServiceTime { get; set; }
    }
    public class clientCaseWorkSelList
    {
        public int idClientCaseHeader { get; set; }
        public int idServiceType { get; set; }
        public string ServiceName { get; set; }
        public bool isCaseWorked { get; set; }
        public DateTime startedDate { get; set; }
    }
    public class clientCaseWorkList
    {
        [Display(Name ="Total No. of Case Worked Events")]
        public int numDetailRecs { get; set; }
        
        [Display(Name ="Total amount of time spent")]
        public TimeSpan totalTimeToDate { get; set; }
        [Display(Name = "Total amount of time spent")]
        public string totalTimeToDateFormatted { get; set; }
        [Display(Name ="Activity")]
        public string ServiceName { get; set; }
        [Display(Name ="Last Staff")]
        public string staffName { get; set; }
        public bool isCaseWorked { get; set; }
        public int idClientCaseDetail { get; set; }
    }
    public class clientNeedsList
    {
        public int idClientNeeds { get; set; }

        public int idClient { get; set; }
        [Display (Name = "Date that these Needs apply")]
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClientNeedsDate { get; set; }
        [Display(Name = "Notes on the Client Needs")]
        [StringLength(65536)]
        public string ClientNeedsNotes { get; set; }
        [Display (Name = "Number of Needs")]
        public int numOfNeeds { get; set; }
    }
    public class caldatList
    {
        public string RegYear { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name="Registration Start Date")]
        public DateTime RegYearStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name="Registration End Date")]
        public DateTime RegYearEndDate { get; set; }

    }
    public class activityList
    {
        public int idClient { get; set; }
        public int idService { get; set; }
        public int idServiceType {get; set;}
        public int journeyDepth { get; set; } // 0 means no journey, 1 means 1st child of parent journey Activity (a.k.a. ServiceType)
        public string ServiceType { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public string JrnyCatName { get; set;}

    }
    public class clist
    {
        public int journeydedidService { get; set; }
        public string serviceTypeName { get; set; }
    }
    public class journeyItem
    {
        public int idclient { get; set; }
        public int origidService { get; set; }
        public int origidServiceType { get; set; }
        public int idServiceType { get; set; }
        public int JourneyDepth { get; set; }
         [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name="Enrolled Date")]
        public DateTime StartedDate { get; set;}
    }
    public class surveyCombinedItems
    {
        public int idClient { get; set; }
        public int idSurvey { get; set; }
        public int idSurResTxt { get; set; }
        public int idSurResNum { get; set; }
        public string seqNo { get; set; }
        public string value { get; set; }
    }
    public class generateList
    {
        public int idSchedules { get; set; }
        public string Resource { get; set; }
        public string ServiceType { get; set; }
        public string lastAttendedDate { get; set; }
        public string lastAttendedTime { get; set;}
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start")]
        public TimeSpan StartTime { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "End")]
        public TimeSpan EndTime { get; set; }

        public string ScheduleType { get; set; }

        [Display(Name = "Rep?")]
        [StringLength(2)]
        
        public string Repetition { get; set; }
        public bool valid { get; set; }

    }
    public class BookingsList
    {
        public int idBookings { get; set; }
        public int idSchedules { get; set; }
        public string Resource { get; set; }
        public int idServiceType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
        public TimeSpan EndTime { get; set; }
        [Display(Name="Marked?")]
        public Boolean Marked { get; set; }
    }
    public class AttendanceMark
    {
        [Key]
        public int idAttendance { get; set; }
        public int idResource { get; set; }
        public int idServiceType { get; set; }
        public int idSchedules { get; set; }
        public int idClient { get; set; }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SessionDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
        public TimeSpan SessionTime { get; set; }
        [Display(Name="No. Times Attended")]
        public int AttendedCount { get; set; }
        [Display(Name = "Present?")]
        public Boolean Present { get; set; }
        [Display(Name = "Sign-in Time")]
        public TimeSpan SignInTime { get; set; }
        [Display(Name = "Sign-out Time")]
        public TimeSpan SignOutTime { get; set; }

    }
    public class EmploymentTracked
    {
        [Key]
        public int idClient { get; set; }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name=("Last Name"))]
        public string LastName { get; set; }
        [Display(Name="Employed?")]
        public Boolean EmployedStatus { get; set; }
        [Display(Name = "Empl Activity Locn")]
        public string EmploymentClubLoc { get; set; }
        [Display(Name="Empl Dest")]
        public string EmploymentDest { get; set; }
       
    }
    public class SurveyEnterList
    {
        [Key]
        public int idSurvey { get; set; }
        public int idClient { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = ("Last Name"))]
        public string LastName { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Num")]
        public string numNQ { get; set; }
        [Display(Name = "Text")]
        public string numTQ { get; set; }
        public Boolean numericReq { get; set; }
        public Boolean textReq { get; set; }
        public Boolean gotResults { get; set; }

    }

    public class SurveyResultsN
    {
        [Key]
        public int idSurResSca { get; set; } // warning key may not exist if any results not committed to database
        public int idSurvey { get; set; }
        public int idClient { get; set; }
        public int questionNum { get; set; }
        public string response { get; set; }
    }
    public class SurveyResultsT
    {
        public int idSurResTxt { get; set; } // warning key may not exist if any results not committed to database
        public int idSurvey { get; set; }
        public int idClient { get; set; }
        [StringLength(3)]
        public string questionNum { get; set; }
        public string response { get; set; }
    }
}