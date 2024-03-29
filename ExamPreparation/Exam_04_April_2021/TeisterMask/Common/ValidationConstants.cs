﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.Common
{
    public static class ValidationConstants
    {
        // Employee
        public const int EmployeeUsernameMinLength = 3;
        public const int EmployeeUsernameMaxLength = 40;

        // Project
        public const int ProjectNameMinLength = 2;
        public const int ProjectNameMaxLength = 40;

        // Task
        public const int TaskNameMinLength = 2;
        public const int TaskNameMaxLength = 40;

        public const int TaskExecutionTypeMinValue = 0;
        public const int TaskExecutionTypeMaxValue = 3;

        public const int TaskLabelTypeMinValue = 0;
        public const int TaskLabelTypeMaxValue = 4;
    }
}
