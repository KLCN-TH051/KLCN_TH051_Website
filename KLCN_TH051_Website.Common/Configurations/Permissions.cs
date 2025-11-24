using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Configurations
{
    public static class Permissions
    {
        public static List<string> All = new()
    {
        // Subject
        "Subject.Create",
        "Subject.Edit",
        "Subject.Delete",
        "Subject.View",

        // Course
        "Course.Create",
        "Course.Edit",
        "Course.Submit",
        "Course.Approve",
        "Course.Delete",
        "Course.ViewAll",
        "Course.View",
        "Course.TeacherView",

        // User
        "User.ViewAll",
        "User.Create",
        "User.Edit",
        "User.Delete",

        // Role
        "Role.Manage",
        "Role.View",

        // TeacherAssignment
        "TeacherAssignment.ViewAll",
        "TeacherAssignment.ViewByTeacher",
        "TeacherAssignment.Create",
        "TeacherAssignment.Edit",
        "TeacherAssignment.Delete",
        "TeacherAssignment.View"
    };
    }
}
