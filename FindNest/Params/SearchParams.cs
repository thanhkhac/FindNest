﻿namespace FindNest.Params
{
    public class SearchParams
    {
        public int PageSize { get; set; } = 20;
        public int CurrentPage { get; set; } = 1;
        public string TextSearch { get; set; } = string.Empty;
        public int StartIndex => (CurrentPage - 1) * PageSize;
    }
}