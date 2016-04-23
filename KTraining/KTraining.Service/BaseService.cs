using KTraining.Data;
using System;
using System.Linq;

namespace KTraining.Service
{
    public class BaseService
    {
        public ApplicationDbContext context;
        public CloudinaryService cloudinaryService;

        public BaseService()
        {
            this.context = new ApplicationDbContext();
            this.cloudinaryService = new CloudinaryService("onlinesystemtesting", "198959495156847", "xaESFiOp5pYOqH4EbQOs_dhnaiY");
        }
    }
}
