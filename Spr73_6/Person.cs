namespace Spr73_6
{
    class Person
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LasttName { get; set; }
        public string Egn { get; set; }

        public Person(string egn)
        {
            this.Egn = egn;
        }
    }
}