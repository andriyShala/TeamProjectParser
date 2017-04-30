namespace ClassLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class Vacancies
    {

        public string Сategory { get; set; }

        public string Sity { get; set; }

        public string Title { get; set; }

        public string Salary { get; set; }

        public string Company { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string WorkSchedule { get; set; }

        public string ContactingInfo { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int? IdVacancysite { get; set; }

        public int? Id_pars_site { get; set; }

        public Dictionary<string, string> s = new Dictionary<string, string>() { { "HR, управление персоналом", "" }, { "IT, WEB специалисты", "" }, { "Банковское дело, ломбарды", "" }, { "Бухгалтерия, финансы, учет/аудит", "" }, { "Гостиничный бизнес", "" }, { "Дизайн, творчество", "" }, { "Домашний сервис", "" }, { "Издательство, полиграфия", "" }, { "Консалтинг", "" }, { "Красота и SPA-услуги", "" }, { "Легкая промышленность", "" }, { "Логистика, доставка, склад", "" }, { "Медицина, фармацевтика", "" }, { "Наука, образование, переводы", "" }, { "Недвижимость и страхование", "" }, { "Офисный персонал", "" }, { "Охрана, безопасность", "" }, { "Производство", "" }, { "Реклама, маркетинг, PR", "" }, { "Ремонт техники и предметов быта", "" }, { "Ресторанный бизнес, кулинария", "" }, { "Руководство", "" }, { "Сельское хозяйство, агробизнес", "" }, { "СМИ, TV, Радио", "" }, { "Строительство, архитектура", "" }, { "Сфера развлечений", "" }, { "Телекоммуникации и связь", "" }, { "Торговля, продажи, закупки", "" }, { "Транспорт, автосервис", "" }, { "Туризм и спорт", "" }, { "Юриспруденция, право", "" }, { "Работа без квалификации", "" }, { "Работа для студентов", "" }, { "Работа за рубежом", "" }, { "Людям с ограниченными возможностями", "" }, { "Другие предложения", "" } };

    }
}
