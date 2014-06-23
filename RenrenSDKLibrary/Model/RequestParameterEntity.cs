﻿//  Copyright 2011年 Renren Inc. All rights reserved.
//  - Powered by Team Pegasus. -

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RenrenSDKLibrary
{
    public class RequestParameterEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public RequestParameterEntity(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
