using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class FcmtokenMobile
    {
        public int Id { get; set; }
        public string TokenId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
