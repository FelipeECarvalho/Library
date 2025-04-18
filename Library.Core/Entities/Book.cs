﻿namespace Library.Core.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        public string ISBN { get; set; }

        public int StockNumber { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
