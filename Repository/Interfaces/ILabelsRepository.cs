using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interfaces
{
    public interface ILabelsRepository
    {
        public Labels CreateLabel(int UserId, LabelsModel model);
        public Labels GetLabelById(int LabelId, int UserId);
        public List<Labels> GetAllLabels(int UserId);
        public bool UpdateLabel(string LabelName, int LabelId, int UserId);
        public bool DeleteLabel(int LabelId, int UserId);
    }
}
