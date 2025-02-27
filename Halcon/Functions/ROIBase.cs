﻿using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public abstract class ROIBase
    {
        private ROIStatus m_ROIStatus = ROIStatus.UNION;

        public HDrawingObject DrawingObject { get; set; }
        public ROIStatus Status { get => m_ROIStatus; set => m_ROIStatus = value; }

        public HRegion GetRegion()
        {
            return new HRegion(DrawingObject.GetDrawingObjectIconic());
        }

        public abstract void CreateDrawingObject();
        public abstract void GenerateParameter();
    }
}
