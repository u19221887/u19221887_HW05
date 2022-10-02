using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u19221887_HW05.Controllers;
using u19221887_HW05.Models;
using u19221887_HW05.Models.ViewModels;


namespace u19221887_HW05.Controllers
{
    public class homeController : Controller
    {



        //Connection string
        SqlConnection Connect = new SqlConnection("Data Source=DESKTOP-T6598KD\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;");



        //Public 
        public static List<bookCombination> Books = new List<bookCombination>();
        public static List<students> Students = new List<students>();
        public static List<borrows> Borrows = new List<borrows>();


        public static List<bookCombination> Check = null;
        public static int CheckBook = 0;

        [HttpGet]
        public ActionResult Index()
        {
            List<bookCombination> returnBooks = null;
            try
            {
                //clear the list
                Books.Clear();
                Borrows.Clear();

                //check to see if exists 
                if (Check != null)
                {
                    returnBooks = Check;
                }
                else
                {
                    // get the books 

                    SqlCommand getBooks = new SqlCommand("SELECT book.[bookId] as bookId ,book.[name] as name ,book.[pagecount] as pagecount ,book.[point] as point, auth.[surname] as authorSurname ,type.[name] typeName " +
                                        "FROM [Library].[dbo].[books] book " +
                                        "JOIN [Library].[dbo].[authors] auth on book.authorId = auth.authorId " +
                                        "JOIN [Library].[dbo].[types] type on book.typeId = type.typeId",
                                        Connect);
                    Connect.Open();

                    //populate
                    SqlDataReader readBooks = getBooks.ExecuteReader();
                    // read from input
                    while (readBooks.Read())
                    {
                        bookCombination book = new bookCombination();
                        book.bookId = (int)readBooks["bookId"];
                        book.name = (string)readBooks["name"];
                        book.authorSurname = (string)readBooks["authorSurname"];
                        book.typeName = (string)readBooks["typeName"];
                        book.pagecount = (int)readBooks["pagecount"];
                        book.point = (int)readBooks["point"];

                        book.status = true;
                        Books.Add(book);
                    }

                    // connectin needs to close 

                    Connect.Close();

                    //get all the students


                    SqlCommand getStudents = new SqlCommand("SELECT * FROM [Library].[dbo].[students]", Connect);
                    Connect.Open();
                    //populate
                    SqlDataReader readStudents = getStudents.ExecuteReader();
                    // read from input
                    while (readStudents.Read())
                    {
                        students student = new students();
                        student.studentId = (int)readStudents["studentId"];
                        student.name = (string)readStudents["name"];
                        student.surname = (string)readStudents["surname"];
                        student.birthdate = (DateTime)readStudents["birthdate"];
                        student.gender = (string)readStudents["gender"];
                        student.Class = (string)readStudents["class"];
                        student.point = (int)readStudents["point"];
                        Students.Add(student);
                    }
                    // connection needs to close 

                    Connect.Close();

                    // get the borrowed ones 

                    SqlCommand getBorrows = new SqlCommand("SELECT * FROM [Library].[dbo].[borrows]", Connect);

                    SqlDataReader readBorrows = getBorrows.ExecuteReader();
                    while (readBorrows.Read())
                    {
                        borrows borrow = new borrows();
                        borrow.borrowId = (int)readBorrows["borrowId"];
                        borrow.studentId = (int)readBorrows["studentId"];
                        borrow.bookId = (int)readBorrows["bookId"];
                        borrow.takenDate = (DateTime)readBorrows["takenDate"];
                        borrow.broughtDate = (DateTime)readBorrows["broughtDate"];
                        Borrows.Add(borrow);
                    }

                    // connection needs to close 

                    Connect.Close();


                    // update books

                    for (int i = 0; i < Borrows.Count; i++)
                    {
                        if (Borrows[i].broughtDate == null)
                        {
                            bookCombination book = Books.Where(x => x.bookId == Borrows[i].bookId).FirstOrDefault();
                            book.status = false;
                            book.studentId = (int)Borrows[i].studentId;
                        }
                    }

                    returnBooks = Books;
                }

                //after the process 

                ViewBag.Types = GetTypes();

                ViewBag.Authors = GetAuthors();
            }
            catch (Exception message)
            {
                ViewBag.Message = message.Message;
            }
            finally
            {
                Connect.Close();
            }

            return View(returnBooks);
        }


          


        private SelectList GetTypes()
        {
            List<types> types = new List<types>();
            SqlCommand getAllTypes = new SqlCommand("SELECT * FROM [Library].[dbo].[types]", Connect);
            Connect.Open();
            SqlDataReader readTypes = getAllTypes.ExecuteReader();
            while (readTypes.Read())
            {
                types type = new types();
                type.typeId = (int)readTypes["typeId"];
                type.name = (string)readTypes["name"];
                types.Add(type);
            }
            Connect.Close();
            return new SelectList(types, "typeId", "name");
        }

        private SelectList GetAuthors()
        {
            List<authors> authors = new List<authors>();
            SqlCommand getAllAuthors = new SqlCommand("SELECT * FROM [Library].[dbo].[authors]", Connect);
            Connect.Open();
            SqlDataReader readAuthors = getAllAuthors.ExecuteReader();
            while (readAuthors.Read())
            {
                authors author = new authors();
                author.authorId = (int)readAuthors["authorId"];
                author.name = (string)readAuthors["name"];
                authors.Add(author);
            }
            return new SelectList(authors, "authorId", "name");
        }




        [HttpGet]
        public ActionResult Details(int bookId)
        {
            bookCombination bookList = Books.Where(x => x.bookId == bookId).FirstOrDefault();// return first element
            if (bookList != null)
            {
                var bookRecords = Borrows.Where(x => x.bookId == bookId).ToList();// convert
                bookList.totalBorrows = bookRecords.Count();
                List<borrowsCombination> RecordOfBorrowed = new List<borrowsCombination>();
                for (int i = 0; i < bookRecords.Count(); i++)
                {
                    borrowsCombination record = new borrowsCombination();
                    record.borrowId = bookRecords[i].borrowId;
                    record.studentName = Students.Where(x => x.studentId == bookRecords[i].studentId).FirstOrDefault().name;
                    record.takenDate = bookRecords[i].takenDate;
                    record.broughtDate = bookRecords[i].broughtDate;
                    RecordOfBorrowed.Add(record);
                }
                bookList.borrowedRecords = RecordOfBorrowed;
            }
            else
            {
                ViewBag.Message = "Book Not Applicable";
            }
            return View(bookList);
        }

        [HttpGet]
        public ActionResult ViewStudents(int bookId)
        {
            bookCombination book = Books.Where(x => x.bookId == bookId).FirstOrDefault();
            ViewBag.Status = book.status;
            if (book.studentId != 0)
            {
                ViewBag.studentId = book.studentId;
            }
            else
            {
                ViewBag.studentId = 0;
            }
            CheckBook = 0;
            CheckBook = bookId;
            return View(Students);
        }
    }
}


