using System;


namespace StudentNameSpace{
    public class Student
    {
        
        private int m_id;
        private string m_nume;
        private string m_facultate;
        private int m_an;

        public Student(int id, string nume, string facultate,int an)
        {
            m_id = id;
            m_an = an;
            m_nume = nume;
            m_facultate = facultate;
        }
        public int ID{get ; set;}
        public string NUME{get; set;}
        public string FACULTATE{get; set;}
        public int AN{get; set;}

    }
}