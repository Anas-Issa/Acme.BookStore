using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Members
{
    public  class CreateUpdateMemberDto
    {
        [Required]
        [MaxLength(128)]
        public string  Name { get; set; }

        public string Notes { get; set; }

        //public List<BorrowBookDto> BorrowBooks { get; set; } = new List<BorrowBookDto>();

    }
}
