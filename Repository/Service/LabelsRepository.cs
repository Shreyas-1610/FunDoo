using CommonLayer.Models;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Service
{
    public class LabelsRepository: ILabelsRepository
    {
        private readonly FunDooDbContext context;
        public LabelsRepository(FunDooDbContext context)
        {
            this.context = context;
        }

        public Labels CreateLabel(int UserId, LabelsModel model)
        {
            Labels label = new Labels();
            label.LabelName = model.LabelName;
            label.NotesId = model.NotesId;
            label.UserId = UserId;

            context.Labels.Add(label);
            context.SaveChanges();

            return label;
        }

        public Labels GetLabelById(int LabelId, int UserId)
        {
            var getLabel = context.Labels.FirstOrDefault(l => l.LabelId == LabelId && l.UserId == UserId);
            if (getLabel != null)
            {
                return getLabel;
            }
            return null;
        }

        public List<Labels> GetAllLabels(int UserId)
        {
            var list = context.Labels.Where(l=>l.UserId == UserId).OrderBy(l=>l.LabelName).ToList();
            return list;
        }

        public bool UpdateLabel(string LabelName,int LabelId, int UserId)
        {
            var labelExist = GetLabelById(LabelId, UserId);
            if (labelExist != null)
            {
                labelExist.LabelName = LabelName;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteLabel(int LabelId, int UserId)
        {
            var checkIfExist = GetLabelById(LabelId, UserId);
            if (checkIfExist != null)
            {
                context.Labels.Remove(checkIfExist);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
