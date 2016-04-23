using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
   public class RequestToJoinViewModel
   {
       public int Id { get; set; }

       public Student SendBy { get; set; }

       public Course Course { get; set; }
   }
}