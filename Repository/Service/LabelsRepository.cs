using Repository.Context;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
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


    }
}
