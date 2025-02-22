using System.Data;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using ISB.Migrations;
using ISB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ISB.Controllers
{

    public class AdminController : Controller
    {
        private readonly dbcontext _con;
        private readonly IWebHostEnvironment _evn;

        public AdminController(dbcontext con, IWebHostEnvironment evn)
        {
            _con = con;
            _evn = evn;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult companyCreatetion()
        {
            var companies_details = _con.tbl_companies.ToList();
            mainModel data = new mainModel()
            {
                companies_details = companies_details
            };
            return View(data);
        }

        [HttpPost]
        public IActionResult companyCreatetion(Companies company, IFormFile logo)
        {
            var name_existing = _con.tbl_companies.FirstOrDefault(p => p.name == company.name);
            if (name_existing == null)
            {

                string file_extension = Path.GetExtension(logo.FileName);
                if (file_extension == ".png" || file_extension == ".jpg" || file_extension == ".jpeg")
                {
                    Random ran = new Random();
                    var currentDate = DateTime.Now;

                    string fileNAme = Path.GetFileName(logo.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = fileNAme = R_num;
                    string file_name = name + file_extension;
                    string filepath = Path.Combine(_evn.WebRootPath, "compnanies_logo", file_name);
                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    logo.CopyTo(fs);
                    company.logo = file_name;
                    company.dateCurrent = currentDate.ToString("yyyy-MM-dd");
                    _con.tbl_companies.Add(company);
                    _con.SaveChanges();
                    TempData["msg1"] = "The company is created now !";

                    return RedirectToAction("companyCreatetion");
                }
                else
                {
                    TempData["msg1"] = "This type of file is not supported !";
                    return RedirectToAction("companyCreatetion");
                }

            }
            else
            {
                TempData["msg1"] = "the name of company is already regsitered!";
                return RedirectToAction("companyCreatetion");
            }

        }

        [HttpGet]
        public IActionResult companies_detail(string search_text)
        {
            List<Companies> company = new List<Companies>();
            if (string.IsNullOrEmpty(search_text))
            {
                company = _con.tbl_companies.ToList();
            }

            else
            {
                company = _con.tbl_companies.FromSqlInterpolated($"select * from tbl_companies where name like '%'+ {search_text}+ '%'").ToList();
            }
            if (company.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            mainModel data = new mainModel() { companies_details = company };
            return View(data);
        }
        public IActionResult delete_comapny(int id)
        {
            var data = _con.tbl_companies.Find(id);
            _con.tbl_companies.Remove(data);
            _con.SaveChanges();
            return RedirectToAction("companies_detail");
        }
        public IActionResult companies_data(int id)
        {
            var company = _con.tbl_companies.Where(p => p.id == id).ToList();
            mainModel data = new mainModel() { companies_details = company };
            return View(data);
        }


        public IActionResult status_comapny(int id)
        {
            var data = _con.tbl_companies.Find(id);
            if (data.status == 1)
            {
                data.status = 0;
            }
            else
            {
                data.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("companies_detail");
        }
        public IActionResult update_comapny(int id)
        {
            var company = _con.tbl_companies.Find(id);
            mainModel data = new mainModel() { company_data = company };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_comapny(Companies company, IFormFile logo)
        {
            string file_extension = Path.GetExtension(logo.FileName);
            if (file_extension == ".png" || file_extension == ".jpg" || file_extension == ".jpeg")
            {
                Random ran = new Random();
                string fileNAme = Path.GetFileName(logo.FileName);
                var R_num = ran.Next(0000, 9999).ToString();
                var name = fileNAme = R_num;
                string file_name = name + file_extension;
                string filepath = Path.Combine(_evn.WebRootPath, "compnanies_logo", file_name);
                FileStream fs = new FileStream(filepath, FileMode.Create);
                logo.CopyTo(fs);
                company.modules = User.FindFirstValue(ClaimTypes.NameIdentifier); // Example for getting user ID

                company.logo = file_name;

                _con.tbl_companies.Update(company);
                _con.SaveChanges();
                TempData["msg"] = "The company is created now !";

                return RedirectToAction("companies_detail");
            }
            else
            {
                TempData["msg"] = "This type of file is not supported !";
                return RedirectToAction("companies_detail");
            }
        }
        [HttpGet]
        public IActionResult roles_details(string search_text)
        {
            List<roles> roles = new List<roles>();
            if (string.IsNullOrEmpty(search_text))
            {
                roles = _con.tbl_roles.ToList();
            }

            else
            {
                roles = _con.tbl_roles.FromSqlInterpolated($"select * from tbl_roles where roles_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (roles.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            mainModel data = new mainModel() { role_detail = roles };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_role(roles role)
        {
            var role_exiting = _con.tbl_roles.FirstOrDefault(p => p.roles_name == role.roles_name);
            if (role_exiting == null)
            {
                _con.tbl_roles.Add(role);
                _con.SaveChanges();
                TempData["msg1"] = "The role is created successfully !";
                return RedirectToAction("roles_details");

            }
            else
            {

                TempData["msg1"] = "The role is already added!";
                return RedirectToAction("roles_details");
            }
        }
        public IActionResult delete_roles(int id)
        {
            var del = _con.tbl_roles.Find(id);
            _con.tbl_roles.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The role has been successfully deleted!";
            return RedirectToAction("roles_details");
        }
        public IActionResult update_roles(int id)
        {
            var find_id = _con.tbl_roles.Find(id);
            mainModel data = new mainModel() { roles_data = find_id };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_roles(roles role)
        {
            var role_exiting = _con.tbl_roles.FirstOrDefault(p => p.roles_name == role.roles_name);
            if (role_exiting == null)
            {
                _con.tbl_roles.Update(role);
                _con.SaveChanges();
                TempData["msg1"] = "The role is updated successfully !";
                return RedirectToAction("roles_details");

            }
            else
            {

                TempData["msg1"] = "The role is already added!";
                return RedirectToAction("roles_details");
            }
        }

        [HttpGet]
        public IActionResult departments_details(string search_text)
        {
            List<depatement> depatements = new List<depatement>();
            if (string.IsNullOrEmpty(search_text))
            {
                depatements = _con.tbl_departments.ToList();
            }

            else
            {
                depatements = _con.tbl_departments.FromSqlInterpolated($"select * from tbl_departments where department_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (depatements.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            mainModel data = new mainModel() { department_details = depatements };
            return View(data);
        }
        public IActionResult view_design(int id)
        {

            var find_id = _con.tbl_designations.Where(p => p.depart_id == id).Include(p => p.depart_detail).ToList();
            if (find_id.Count == 0)
            {
                TempData["msg"] = "there is no designation is appointed to this department";
            }
            mainModel data = new mainModel() { design_details = find_id };
            return View(data);
        }
        public IActionResult update_design(int id)
        {
            var find_id = _con.tbl_designations.Find(id);
            var department_list = _con.tbl_departments.ToList();
            mainModel data = new mainModel()
            {
                design_data = find_id,
                department_details = department_list
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_design(designation design)
        {
            _con.tbl_designations.Update(design);
            _con.SaveChanges();
            TempData["msg1"] = "The designation is update successfully !";
            return RedirectToAction("departments_details");
        }
        [HttpPost]
        public IActionResult add_department(depatement depart)
        {
            var depart_exiting = _con.tbl_departments.FirstOrDefault(p => p.department_name == depart.department_name);
            if (depart_exiting == null)
            {
                _con.tbl_departments.Add(depart);
                _con.SaveChanges();
                TempData["msg1"] = "The department is created successfully !";
                return RedirectToAction("departments_details");

            }
            else
            {

                TempData["msg1"] = "The department is already added!";
                return RedirectToAction("departments_details");
            }
        }
        public IActionResult delete_depart(int id)
        {
            var del = _con.tbl_departments.Find(id);
            _con.tbl_departments.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The department has been successfully deleted!";
            return RedirectToAction("departments_details");
        }
        public IActionResult delete_design(int id)
        {
            var del = _con.tbl_designations.Find(id);
            _con.tbl_designations.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The department has been successfully deleted!";
            return RedirectToAction("departments_details");
        }
        public IActionResult update_depart(int id)
        {
            var find_id = _con.tbl_departments.Find(id);
            mainModel data = new mainModel() { department_data = find_id };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_depart(depatement depart)
        {
            var depart_exiting = _con.tbl_departments.FirstOrDefault(p => p.department_name == depart.department_name);
            if (depart_exiting == null)
            {
                _con.tbl_departments.Update(depart);
                _con.SaveChanges();
                TempData["msg1"] = "The department is updated successfully !";
                return RedirectToAction("departments_details");

            }
            else
            {

                TempData["msg1"] = "The department is already added!";
                return RedirectToAction("departments_details");
            }
        }
        [HttpPost]
        public IActionResult add_designation(designation design)
        {
            var design_exiting = _con.tbl_designations.FirstOrDefault(p => p.designation_name == design.designation_name);
            if (design_exiting == null)
            {
                _con.tbl_designations.Add(design);
                _con.SaveChanges();
                TempData["msg1"] = "The designation is updated successfully !";
                return RedirectToAction("departments_details");

            }
            else
            {

                TempData["msg1"] = "The designation is already added!";
                return RedirectToAction("departments_details");
            }

        }




        [HttpGet]
        public IActionResult employee_details(string search_text)
        {
            List<employee> emp_data = new List<employee>();
            if (string.IsNullOrEmpty(search_text))
            {
                emp_data = _con.tbl_employee.ToList();
            }

            else
            {
                emp_data = _con.tbl_employee.FromSqlInterpolated($"select * from tbl_employee where name like '%'+ {search_text}+ '%' or email like '%'+ {search_text}+ '%'").ToList();
            }
            if (emp_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            var designation = _con.tbl_designations.ToList();
            var department = _con.tbl_departments.ToList();
            var roles = _con.tbl_roles.ToList();
            var material_status = _con.tbl_maritalstatus.Where(p => p.status == 1).ToList();
            var _type = _con.tbl_emptypes.Where(p => p.status == 1).ToList();
        
            mainModel data = new mainModel()
            { 
             
                emp_types_details=_type,
                employee_details = emp_data,
                role_detail = roles,
                materialStatus_details=material_status,
                department_details = department,
                design_details = designation,

            };
            return View(data);
        }

        [HttpPost]
        public IActionResult add_emp(employee emp, IFormFile iamge, int length)
        {
            // Check if the email already exists
            var existingEmployee = _con.tbl_employee.FirstOrDefault(p => p.email == emp.email);

            if (existingEmployee == null)
            {
                // Validate the image file extension
                string fileExtension = Path.GetExtension(iamge.FileName);
                if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    // Generate a unique file name
                    string fileName = $"{Guid.NewGuid()}{fileExtension}"; // Use GUID for uniqueness
                    string filePath = Path.Combine(_evn.WebRootPath, "emp_images", fileName);

                    // Save the image file
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.CopyTo(fs);
                    }
                    string pass = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    Random rd = new Random();
                    char[] mypass = new char[7];
                    for (int i = 0; i < 7; i++)
                    {
                        mypass[i] = pass[(int)(35 * rd.NextDouble())];
                    }
                    string mypassString = new string(mypass);
                    emp.password = mypassString;



                    emp.iamge = fileName; // Save the file name in the employee record
                    emp.status = 1;
                    // Add the employee to the database
                    _con.tbl_employee.Add(emp);
                    _con.SaveChanges();

                    TempData["msg1"] = "The employee has been successfully added!";
                    return RedirectToAction("EmployeeDetails");
                }
                else
                {
                    TempData["msg1"] = "This type of file is not supported!";
                    return RedirectToAction("EmployeeDetails");
                }
            }
            else
            {
                TempData["msg1"] = "An employee with this email already exists!";
                return RedirectToAction("EmployeeDetails");
            }
        }
        public IActionResult login_form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login_form(string logined_email, string logined_pass)
        {
            var user_logined = _con.tbl_employee.FirstOrDefault(e => e.email == logined_email);
            if (user_logined != null && user_logined.password == logined_pass)
            {
                if (user_logined.status == 1)
                {
                    HttpContext.Session.SetString("user_session", user_logined.id.ToString());
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["logined"] = "The account is block by the admin";
                    return RedirectToAction("login_form");
                }
            }
            else
            {
                TempData["logined"] = "The email or password is block by the admin";
                return RedirectToAction("login_form");
            }

        }

        public IActionResult emp_data(int id)
        {
            var find_id = _con.tbl_employee.Where(p => p.id == id).ToList();
            var data_emp = _con.tbl_employee.FirstOrDefault(p => p.id == id);
            var roles_item = _con.tbl_roles.Where(p => p.id == data_emp.role_id).ToList();
            var depart_item = _con.tbl_departments.Where(p => p.id == data_emp.depart_id).ToList();
            var desgni_item = _con.tbl_designations.Where(p => p.id == data_emp.designationid).ToList();
            mainModel data = new mainModel()
            {
                employee_details = find_id,
                role_detail = roles_item,
                department_details = depart_item,
                design_details = desgni_item

            };
            return View(data);
        }
        public IActionResult status_emp(int id)
        {
            var data = _con.tbl_employee.Find(id);
            if (data.status == 1)
            {
                data.status = 0;
            }
            else
            {
                data.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("employee_details");
        }
        public IActionResult delete_emp(int id)
        {
            var del = _con.tbl_employee.Find(id);
            _con.tbl_employee.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The employee has been successfully deleted!";
            return RedirectToAction("employee_details");
        }
        public IActionResult update_emp(int id)
        {
            var find_id = _con.tbl_employee.Find(id);
            var data_emp = _con.tbl_employee.FirstOrDefault(p => p.id == id);
            var roles_item = _con.tbl_roles.ToList();
            var selected_role = _con.tbl_roles.FirstOrDefault(r => r.id == find_id.role_id);
            var depart_item = _con.tbl_departments.ToList();
            var selected_depart = _con.tbl_departments.FirstOrDefault(r => r.id == find_id.depart_id);
            var desgni_item = _con.tbl_designations.ToList();
            var selected_design = _con.tbl_designations.FirstOrDefault(r => r.id == find_id.designationid);
            var material_status = _con.tbl_maritalstatus.Where(p => p.status == 1).ToList();
            var _type = _con.tbl_emptypes.Where(p => p.status == 1).ToList();
        
            mainModel data = new mainModel()
            {
                design_data=selected_design,
                emp_types_details = _type,
                department_data=selected_depart,
                roles_data=selected_role,
         
                materialStatus_details = material_status,
                employee_data = find_id,
                role_detail = roles_item,
                department_details = depart_item,
                design_details = desgni_item

            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_emp(employee emp, IFormFile iamge)
        {
            string file_extension = Path.GetExtension(iamge.FileName);
            if (file_extension == ".png" || file_extension == ".jpg" || file_extension == ".jpeg")
            {
                Random ran = new Random();
                string fileNAme = Path.GetFileName(iamge.FileName);
                var R_num = ran.Next(0000, 9999).ToString();
                var name = fileNAme = R_num;
                string file_name = name + file_extension;
                string filepath = Path.Combine(_evn.WebRootPath, "emp_images", file_name);
                FileStream fs = new FileStream(filepath, FileMode.Create);
                iamge.CopyTo(fs);
                //company.modules = User.FindFirstValue(ClaimTypes.NameIdentifier); // Example for getting user ID
                emp.status = 1;
                emp.iamge = file_name;

                _con.tbl_employee.Add(emp);
                _con.SaveChanges();
                TempData["msg1"] = "The employee is successfully updated now !";

                return RedirectToAction("employee_details");
            }
            else
            {
                TempData["msg1"] = "This type of file is not supported !";
                return RedirectToAction("employee_details");
            }
        }
        /*for the client*/
        [HttpGet]
        public IActionResult client_details(string search_text)
        {
            List<client> client_data = new List<client>();
            if (string.IsNullOrEmpty(search_text))
            {
                client_data = _con.tbl_clients.ToList();
            }

            else
            {
                client_data = _con.tbl_clients.FromSqlInterpolated($"select * from tbl_clients where name like '%'+ {search_text}+ '%' or email like '%'+ {search_text}+ '%'").ToList();
            }
            if (client_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }

            mainModel data = new mainModel()
            {
                client_details = client_data

            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_client(client _clinet)
        {
            var number_exisiting = _con.tbl_clients.FirstOrDefault(p => p.number == _clinet.number);
            if (number_exisiting == null)
            {
                _con.tbl_clients.Add(_clinet);
                _con.SaveChanges();
                TempData["msg1"] = "The client is successfully added ";
                return RedirectToAction("client_details");
            }
            else
            {
                TempData["msg1"] = "The client is  added  with this number " + " " + _clinet.number;
                return RedirectToAction("client_details");
            }


        }
        public IActionResult delete_client(int id)
        {
            var del = _con.tbl_clients.Find(id);
            _con.tbl_clients.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The Client has been successfully deleted!";
            return RedirectToAction("client_details");
        }
        public IActionResult update_client(int id)
        {
            var clintdata = _con.tbl_clients.Find(id);

            mainModel data = new mainModel()
            {
                client_data = clintdata

            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_client(client _client)
        {
            _con.tbl_clients.Update(_client);
            _con.SaveChanges();
            TempData["msg1"] = "The Client has been successfully updated!";
            return RedirectToAction("client_details");
        }
        [HttpGet]
        public IActionResult team_details(string search_text)
        {

            List<team> team_data = new List<team>();
            if (string.IsNullOrEmpty(search_text))
            {
                team_data = _con.tbl_teams.ToList();
            }

            else
            {
                team_data = _con.tbl_teams.FromSqlInterpolated($"select * from tbl_teams where team_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (team_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            var employee = _con.tbl_employee.ToList();
            mainModel data = new mainModel()
            {
                employee_details = employee,
                team_details = team_data
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_team(team _team)
        {
            _con.tbl_teams.Add(_team);
            _con.SaveChanges();
            TempData["msg1"] = "The team has been successfully created!";
            return RedirectToAction("team_details");
        }
        public IActionResult delete_team(int id)
        {
            var del = _con.tbl_teams.Find(id);
            _con.tbl_teams.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The team has been successfully deleted!";
            return RedirectToAction("team_details");
        }
        public IActionResult update_teamstatus(int id)
        {
            var data = _con.tbl_teams.Find(id);
            if (data.status == 1)
            {
                data.status = 0;
            }
            else
            {
                data.status = 1;
            }
            _con.SaveChanges();
            TempData["msg1"] = "The status of team has been successfully updated!";
            return RedirectToAction("team_details");
        }
        public IActionResult update_team(int id)
        {
            var teamdata = _con.tbl_teams.Find(id);
            var employee = _con.tbl_employee.ToList();
            mainModel data = new mainModel()
            {
                team_data = teamdata,
                employee_details = employee,
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_team(team _team)
        {
            _con.tbl_teams.Update(_team);
            _con.SaveChanges();
            TempData["msg1"] = "The  team has been successfully updated!";
            return RedirectToAction("team_details");
        }
        [HttpGet]
        public IActionResult project_details(string search_text)
        {

            List<project> pro_data = new List<project>();
            if (string.IsNullOrEmpty(search_text))
            {
                pro_data = _con.tbl_projects.Include(p => p.employee_details).Include(p=>p.status_details).ToList();
            }

            else
            {
                pro_data = _con.tbl_projects.FromSqlInterpolated($"select * from tbl_teams where team_name like '%'+ {search_text}+ '%'").Include(p => p.status_details).ToList();
            }
            if (pro_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            var team_details = _con.tbl_teams.Where(p => p.status == 1).ToList();
            var emp_data = _con.tbl_employee.Where(p => p.status == 1).ToList();
            var client_details = _con.tbl_clients.ToList();
            var status = _con.tbl_projectStatus.Where(p => p.status == 1).ToList();
            var priority = _con.tbl_projectpriority.Where(p => p.status == 1).ToList(); 
            var type  = _con.tbl_project_types.Where(p => p.status == 1).ToList();
          
            mainModel data = new mainModel()
            {
                projects_types=type,
                projects_prioritesDetails=priority,
                pro_Statusdetail=status,
                project_details = pro_data,
                employee_details=emp_data,
                team_details = team_details,
                client_details = client_details,
            };
            return View(data);
        }
        [HttpPost]

        public IActionResult add_project(project pro, IFormFile document)
        {
            if (document != null) // Check if a file was actually uploaded
            {
                string file_extension = Path.GetExtension(document.FileName);
                if (file_extension == ".pdf" || file_extension == ".docx" || file_extension == ".txt")
                {
                    Random ran = new Random();
                    string fileNAme = Path.GetFileName(document.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = fileNAme = R_num; // This line seems redundant; you're re-assigning fileNAme
                    string file_name = name + file_extension;
                    string filepath = Path.Combine(_evn.WebRootPath, "documents", file_name);
                    using (FileStream fs = new FileStream(filepath, FileMode.Create)) // Use 'using' for proper disposal
                    {
                        document.CopyTo(fs);
                    }

                    pro.document = file_name; // Store the path in the project object
                }
                else
                {
                    TempData["msg1"] = "This type of file is not supported !";
                    return RedirectToAction("project_details");
                }
            }
            // If document is null, the code will skip the file processing block.  pro object will be saved without file info.

            _con.tbl_projects.Add(pro);
            _con.SaveChanges();
            TempData["msg1"] = "The project is successfully updated now !";

            return RedirectToAction("project_details");

        }
        public IActionResult delete_project(int id)
        {
            var data = _con.tbl_projects.Find(id);
            _con.tbl_projects.Remove(data);
            _con.SaveChanges();
            return RedirectToAction("project_details");
        }
        //public IActionResult update_prostatus(int id)
        //{
        

        //}
        public IActionResult project_status()
        {
            var status = _con.tbl_projectStatus.ToList();
          
            
            mainModel data = new mainModel()
            {
              pro_Statusdetail=status,
            };
            return View(data);

        }
        [HttpPost]
        public IActionResult add_projectStatus(project_status _status)
        {
            _con.tbl_projectStatus.Add(_status);
            
            _con.SaveChanges();
            TempData["msg1"] = "The project Status is successfully added!";
            return RedirectToAction("project_status");

        }
        public  IActionResult delete_statusProj(int id)
        {
            var data = _con.tbl_projectStatus.Find(id);
            _con.tbl_projectStatus.Remove(data);
            _con.SaveChanges();
            return RedirectToAction("project_status");

        }
        public IActionResult update_statusProj(int id)
        {
            var find_id = _con.tbl_projectStatus.Find(id);
           
            mainModel data = new mainModel()
            {
                proStatus_data=find_id
              
            };
            return View(data);
        }
        public IActionResult update_project(int id)
        {
            var find_id = _con.tbl_projects.Find(id);
            var team_details = _con.tbl_teams.Where(p => p.status == 1).ToList();
            var emp_data = _con.tbl_employee.Where(p => p.status == 1).ToList();
            var client_details = _con.tbl_clients.ToList();
            var status = _con.tbl_projectStatus.Where(p => p.status == 1).ToList();
            var priority = _con.tbl_projectpriority.Where(p => p.status == 1).ToList();
            var type = _con.tbl_project_types.Where(p => p.status == 1).ToList();
            var client_name = _con.tbl_clients.FirstOrDefault(P => P.id ==find_id.client_id);
            var type_name = _con.tbl_project_types.FirstOrDefault(P => P.id == find_id.project_types); 
            var employe_name = _con.tbl_employee.FirstOrDefault(P => P.id == find_id.team_id);
            var priority_name = _con.tbl_projectpriority.FirstOrDefault(P => P.id== find_id.project_priority);
            var status_name = _con.tbl_projectStatus.FirstOrDefault(P => P.status_id== find_id.project_statuss);
            mainModel data = new mainModel()
            {
                client_data=client_name,
                projects_types = type,
                projects_prioritesDetails = priority,
                pro_Statusdetail = status,
               
                employee_details = emp_data,
                team_details = team_details,
                client_details = client_details,
                projects_typesData=type_name,
                employee_data=employe_name,
                projects_prioritesData=priority_name,
                proStatus_data=status_name,
                

         
            project_data = find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_project(project_status _status)
        {
            
            _con.tbl_projectStatus.Update(_status);
            _con.SaveChanges();
            return RedirectToAction("project_status");

        }
        public IActionResult updatestatus_statusProj(int id)
        {
           var find_id= _con.tbl_projectStatus.Find(id);
            if (find_id.status == 1)
            {
                find_id.status = 0;

            }
            else
            {
                find_id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("project_status");

        }
        public IActionResult survey_details(string search_text)
        {

            List<survey> survey_data = new List<survey>();
            if (string.IsNullOrEmpty(search_text))
            {
                survey_data = _con.tbl_survey.Include(p => p.employee_details).ToList();
            }

            else
            {
                survey_data = _con.tbl_survey.FromSqlInterpolated($"select * from tbl_survey  where subject like '%'+ {search_text}+ '%'").Include(p => p.employee_details).ToList();
            }
            if (survey_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }

            var emp_details = _con.tbl_employee.ToList();
            mainModel data = new mainModel()
            {
                employee_details = emp_details,
                survey_details = survey_data
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_survey(survey _survey)
        {
            _con.tbl_survey.Add(_survey);
            _con.SaveChanges();
            TempData["msg1"] = "The survey is successfully added!";
            return RedirectToAction("survey_details");
        }
        public IActionResult delete_survey(int id)
        {
            var del = _con.tbl_survey.Find(id);
            _con.tbl_survey.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The survey is successfully deleted!";
            return RedirectToAction("survey_details");
        }
        public IActionResult update_survey(int id)
        {
            var find_id = _con.tbl_survey.Find(id);
            var emp_details = _con.tbl_employee.ToList();
            var emp_data = _con.tbl_employee.Where(e => e.id == find_id.emp_id);
            var survey = _con.tbl_survey.Where(p => p.id == id).ToList();
            mainModel data = new mainModel()
            {
                employee_details = emp_details,
                survey_details = survey,
                survey_data = find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_survey(survey _survey)
        {
            _con.tbl_survey.Update(_survey);
            _con.SaveChanges();
            TempData["msg1"] = "The survey is successfully updated!";
            return RedirectToAction("survey_details");
        }

        [HttpGet]
        public IActionResult Impact_risk(string search_text)
        {
            List<Impact_risk> Impact_risk_data = new List<Impact_risk>();
            if (string.IsNullOrEmpty(search_text))
            {
                Impact_risk_data = _con.tbl_impact_risk.ToList();
            }

            else
            {
                Impact_risk_data = _con.tbl_impact_risk.FromSqlInterpolated($"select * from tbl_impact_risk  where risk_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (Impact_risk_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }

            var emp_details = _con.tbl_employee.ToList();
            mainModel data = new mainModel()
            {
                impact_Risks=Impact_risk_data
            };
            return View(data);
            
        }
        [HttpPost]
        public IActionResult add_impactRisk(Impact_risk _risk)
        {
            _con.tbl_impact_risk.Add(_risk);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully added!";
            return RedirectToAction("Impact_risk");
        }
        public  IActionResult delete_impactRisk (int id)
        {
            var del = _con.tbl_impact_risk.Find(id);
            _con.tbl_impact_risk.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully deleted!";
            return RedirectToAction("Impact_risk");
        }
        public IActionResult update_ImpactRisk(int id)
        {

            var find_id = _con.tbl_impact_risk.Find(id);
            mainModel data = new mainModel()
            {
              impactrisk_data=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_ImpactRisk(Impact_risk _risk)
        {
            _con.tbl_impact_risk.Update(_risk);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully updated now!";
            return RedirectToAction("Impact_risk");
        }
        [HttpGet]
        public IActionResult likehood_risk(string search_text)
        {
            List<Likelihood_risk> Likelihood_risk_data = new List<Likelihood_risk>();
            if (string.IsNullOrEmpty(search_text))
            {
               Likelihood_risk_data= _con.tbl_likedhood_risk.ToList();
            }

            else
            {
                Likelihood_risk_data = _con.tbl_likedhood_risk.FromSqlInterpolated($"select * from tbl_likedhood_risk  where risk_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (Likelihood_risk_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }

            var emp_details = _con.tbl_employee.ToList();
            mainModel data = new mainModel()
            {
                Likelihood_risk=Likelihood_risk_data
            };
            return View(data);

        }
        [HttpPost]
        public IActionResult add_likelihoodRisk(Likelihood_risk _risk)
        {
            _con.tbl_likedhood_risk.Add(_risk);
          
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully added!";
            return RedirectToAction("likehood_risk");
        }
        public IActionResult delete_likehoodRisk(int id)
        {
           var del= _con.tbl_likedhood_risk.Find(id);
            _con.tbl_likedhood_risk.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully deleted!";
            return RedirectToAction("likehood_risk");
        }
        public IActionResult update_likehoodRisk(int id)
        {
            var find_id = _con.tbl_likedhood_risk.Find(id);
            mainModel data = new mainModel()
            {
              likelihoodrisk_data=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_likehoodRisk(Likelihood_risk _risk)
        {
            _con.tbl_likedhood_risk.Update(_risk);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully updated now!";
            return RedirectToAction("likehood_risk");
        }
        [HttpGet]
        public IActionResult risk_details(string search_text)
        {
            List<risk> _risk_data = new List<risk>();
            if (string.IsNullOrEmpty(search_text))
            {
                _risk_data = _con.tbl_risks.ToList();
            }

            else
            {
               _risk_data = _con.tbl_risks.FromSqlInterpolated($"select * from tbl_risks  where risk_name like '%'+ {search_text}+ '%'").ToList();
            }
            if (_risk_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }

            var impact_risk = _con.tbl_impact_risk.ToList();
            var likelihood_liked = _con.tbl_likedhood_risk.ToList();
            var risktype = _con.tbl_risk_types.ToList();
            mainModel data = new mainModel()
            {
              risk_details=_risk_data,
              impact_Risks=impact_risk,
              Risktypes_details=risktype,
              Likelihood_risk=likelihood_liked
            };
            return View(data);

        }

        [HttpPost]
        public IActionResult add_risk(risk _risk)
        {
            _risk.total_scoring = _risk.impact_risk * _risk.likelihood_risk;

            _con.tbl_risks.Add(_risk);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully added now!";
            return RedirectToAction("risk_details");
        }
        public IActionResult delete_Risk(int id)
        {
            var del = _con.tbl_risks.Find(id);
            _con.tbl_risks.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully deleted!";
            return RedirectToAction("risk_details");
        }
        public IActionResult update_Risk(int id)
        {
            var find_id = _con.tbl_risks.Find(id);
            var impact_risk = _con.tbl_impact_risk.ToList();
            var likelihood_liked = _con.tbl_likedhood_risk.ToList();
            mainModel data = new mainModel()
            {
              risk_data=find_id,
                impact_Risks = impact_risk,
                Likelihood_risk = likelihood_liked
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_Risk(risk _risk)
        {
            _risk.total_scoring = _risk.impact_risk * _risk.likelihood_risk;
            _con.tbl_risks.Update(_risk);
            _con.SaveChanges();
            TempData["msg1"] = "The risk is successfully updated!";
            return RedirectToAction("risk_details");
        }
        public IActionResult project_type()
        {
            var type = _con.tbl_project_types.ToList();


            mainModel data = new mainModel()
            {
               projects_types=type,
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_projectType(project_type _type)
        {
            _con.tbl_project_types.Add(_type);

            _con.SaveChanges();
            TempData["msg1"] = "The project type is successfully added!";
            return RedirectToAction("project_type");

        }
        public IActionResult delete_projecttype(int id)
        {
            var del = _con.tbl_project_types.Find(id);
            _con.tbl_project_types.Remove(del);
            _con.SaveChanges();
            TempData["msg1"] = "The project type is successfully deleted!";
            return RedirectToAction("project_type");
        }
        public IActionResult update_projecttype(int id)
        {
            var find_id = _con.tbl_project_types.Find(id);
            mainModel data = new mainModel()
            {
           projects_typesData=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_projecttype(project_type _type)
        {
            _con.tbl_project_types.Update(_type);
            _con.SaveChanges();
            return RedirectToAction("project_type");

        }
       public IActionResult task_type()
        {
            
                var type = _con.tbl_task_types.ToList();


                mainModel data = new mainModel()
                {
                   task_typedetails=type
                };
                return View(data);

        }

     
        [HttpPost]
        public IActionResult add_taskType(task_type _type)
        {
            _con.tbl_task_types.Add(_type);
            _con.SaveChanges();
            return RedirectToAction("task_type");
        }
        public IActionResult delete_tasktype(int id)
        {
            var del = _con.tbl_task_types.Find(id);
            _con.tbl_task_types.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("task_type");
        }
        public IActionResult update_tasktype(int id)
        {
            var find_id = _con.tbl_task_types.Find(id);
            mainModel data = new mainModel()
            {
              task_typedata=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_tasktype(task_type _type)
        {
            _con.tbl_task_types.Update(_type);
            _con.SaveChanges();
            return RedirectToAction("task_type");

        }
        public IActionResult taskstatus()
        {

            var status = _con.tbl_task_status.ToList();
                  

            mainModel data = new mainModel()
            {
            task_statusdetails=status
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_taskstatus(task_status _status)
        {
            _con.tbl_task_status.Add(_status);
            _con.SaveChanges();
            return RedirectToAction("taskstatus");
        }
        public IActionResult delete_taskststatus(int id)
        {
            var del = _con.tbl_task_status.Find(id);
            _con.tbl_task_status.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("taskstatus");
        }
        public IActionResult update_taskstatus(int id)
        {
            var find_id = _con.tbl_task_status.Find(id);
            mainModel data = new mainModel()
            {
                task_statusdata=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_taskstatus(task_status _status)
        {
            _con.tbl_task_status.Update(_status);
            _con.SaveChanges();
            return RedirectToAction("taskstatus");
        }

        public IActionResult update_tasks_status(int id)
        {
           var find_id= _con.tbl_task_status.Find(id);
            if (find_id.status == 1)
            {
                find_id.status = 0;
            }
            else
            {
                find_id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("taskstatus");
        }
        public IActionResult project_priority()
        {
            var priority = _con.tbl_projectpriority.ToList();


            mainModel data = new mainModel()
            {
               projects_prioritesDetails=priority
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult addproject_priority(project_priority _priority)
        {
            _con.tbl_projectpriority.Add(_priority);
            _con.SaveChanges();
            return RedirectToAction("project_priority");
        }
        public IActionResult delete_projectpriority(int id)
        {
            var del = _con.tbl_projectpriority.Find(id);
            _con.tbl_projectpriority.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("project_priority");
        }
        public IActionResult update_projectpriority(int id)
        {
            var find_id = _con.tbl_projectpriority.Find(id);
            mainModel data = new mainModel()
            {
               projects_prioritesData=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_projectpriority(project_priority _prority)
        {
            _con.tbl_projectpriority.Update(_prority);
            _con.SaveChanges();
            return RedirectToAction("project_priority");

        }
        public IActionResult projectPriorityStatus(int id)
        {
            var find_id = _con.tbl_projectpriority.Find(id);
            if (find_id.status == 1)
            {
                find_id.status = 0;
            }
            else
            {
                find_id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("project_priority");
        }
        public IActionResult task_priority()
        {
            var task_priority= _con.tbl_taskpriority.ToList();


            mainModel data = new mainModel()
            {
              task_prioritesDetails=task_priority
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult addtask_priority(task_priority _Priority)
        {
            _con.tbl_taskpriority.Add(_Priority);
            _con.SaveChanges();
            return RedirectToAction("task_priority");
        }
        public IActionResult delete_taskprority(int id)
        {
            var del = _con.tbl_taskpriority.Find(id);
            _con.tbl_taskpriority.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("task_priority");
        }

        public IActionResult projecttaskprioritiesStatus(int id)
        {
            var find_id = _con.tbl_taskpriority.Find(id);
            if (find_id.status == 1)
            {
                find_id.status = 0;
            }
            else
            {
                find_id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("task_priority");
        }
        public IActionResult update_taskpriority(int id)
        {
            var find_id = _con.tbl_taskpriority.Find(id);
            mainModel data = new mainModel()
            {
               task_prioritesData=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_taskpriority(task_priority _task)
        {
            _con.tbl_taskpriority.Update(_task);
            _con.SaveChanges();
            return RedirectToAction("task_priority");

        }
        public IActionResult task_details(string search_text)
        {
            List<task> task_data = new List<task>();
            if (string.IsNullOrEmpty(search_text))
            {
                task_data = _con.tbl_tasks.Include(p=>p.emp_data).Include(a=>a._status).ToList();
            }

            else
            {
                task_data = _con.tbl_tasks.FromSqlInterpolated($"select * from tbl_project_task  where name like '%'+ {search_text}+ '%'").Include(p => p.emp_data).Include(a => a._status).ToList();
            }
            if (task_data.Count == 0)
            {
                TempData["msg"] = $"sorry the record is not found {search_text} ";
            }
            var team_details = _con.tbl_teams.Where(p => p.status == 1).ToList();
            var emp = _con.tbl_employee.Where(p => p.status == 1).ToList();
            var project_details = _con.tbl_projects.ToList();
            var _types = _con.tbl_task_types.Where(p => p.status == 1).ToList();
            var _status = _con.tbl_task_status.Where(p => p.status == 1).ToList();
            var _prority = _con.tbl_taskpriority.Where(p => p.status == 1).ToList();
            var _risk = _con.tbl_risks.ToList();
            mainModel data = new mainModel()
            {
                employee_details=emp,
                risk_details=_risk,
                task_details = task_data,
                task_prioritesDetails=_prority,
                task_typedetails=_types,
                task_statusdetails=_status,
                project_details = project_details,
                team_details = team_details
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult add_Task(task _task, IFormFile document)
        {
            if (document != null) // Check if a file was actually uploaded
            {
                string file_extension = Path.GetExtension(document.FileName);
                if (file_extension == ".pdf" || file_extension == ".docx" || file_extension == ".txt")
                {
                    Random ran = new Random();
                    string fileNAme = Path.GetFileName(document.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = fileNAme = R_num; // This line seems redundant; you're re-assigning fileNAme
                    string file_name = name + file_extension;
                    string filepath = Path.Combine(_evn.WebRootPath, "documents", file_name);
                    using (FileStream fs = new FileStream(filepath, FileMode.Create)) // Use 'using' for proper disposal
                    {
                        document.CopyTo(fs);
                    }

                    _task.document = file_name;
                    // Store the path in the project object
                }
                else
                {
                    TempData["msg1"] = "This type of file is not supported !";
                    return RedirectToAction("task_details");
                }
            }
            // If document is null, the code will skip the file processing block.  pro object will be saved without file info.
            _task.created_date = DateTime.Now.ToString();
            _task.updated_date = DateTime.Now.ToString();
            _con.tbl_tasks.Add(_task);
            _con.SaveChanges();
            TempData["msg1"] = "The Task is successfully updated now !";
            return RedirectToAction("task_details");
        }
        public IActionResult delete_task(int id)
        {
           var del=  _con.tbl_tasks.Find(id);
            _con.tbl_tasks.Remove(del);
            return RedirectToAction("task_details");
        }
        public IActionResult view_task(int id)
        {

            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var logined_user = _con.tbl_employee.Where(p => p.id ==int.Parse(login)).ToList();
                var task_data = _con.tbl_tasks.Where(p => p.task_id == id).ToList();
                var task_view = _con.tbl_tasks.Find(id);
                var team_details = _con.tbl_teams.Where(p => p.status == 1).ToList();
                var _emp = _con.tbl_employee.FirstOrDefault(p => p.id == task_view.employee_ids);
               
                var project_details = _con.tbl_projects.FirstOrDefault(p => p.Project_id == task_view.project_);
                var _types = _con.tbl_task_types.FirstOrDefault(p => p.id == task_view.task_types);
                var _status = _con.tbl_task_status.FirstOrDefault(p => p.id == task_view.task_statuss);
                var _prority = _con.tbl_taskpriority.FirstOrDefault(p => p.id == task_view.task_priority);
                var _risk = _con.tbl_risks.ToList();
                var employe = _con.tbl_employee.Where(e=>e.status==1).ToList();
                var _files=_con.tbl_taks_file.Where(u=>u.task_ids==id).ToList();
                if (_files.Count == 0) {
                    TempData["nofound"] = "there is not any file is upload";
                }
                mainModel data = new mainModel()
                {
                    logined_user = logined_user,
                   employee_data=_emp,
                 
                    task_data = task_view,
                    risk_details = _risk,
                    employee_details=employe,
                    task_details = task_data,
                    task_prioritesData = _prority,
                    task_typedata = _types,
                    task_statusdata = _status,
                    project_data = project_details,
                    team_details = team_details,
                    files_details=_files
                    
                };
                return View(data);
            }
            else
            {
                return RedirectToAction("login_form");
            }


       

        }
   
        public IActionResult update_tasks(int id)
        {
            //_con.tbl_tasks.Find(task_id);
            var task_datas = _con.tbl_tasks.Find(id);
       var project=_con.tbl_projects.ToList();
            var risk = _con.tbl_risks.ToList();
            var _taskstatus = _con.tbl_task_status.Where(p => p.status == 1).ToList();
            var status = _con.tbl_task_status.FirstOrDefault(p => p.id == task_datas.task_statuss);
           
            var project_info = _con.tbl_projects.FirstOrDefault(p => p.Project_id == task_datas.project_);
            var type = _con.tbl_task_types.Where(a => a.status == 1).ToList();
            var _typedata = _con.tbl_task_types.FirstOrDefault(t => t.id == task_datas.task_types);
            var _priority = _con.tbl_taskpriority.Where(p => p.status == 1).ToList();

            var _priority_selected = _con.tbl_taskpriority.FirstOrDefault(p => p.id == task_datas.task_priority);
            var _emp = _con.tbl_employee.Where(a => a.status == 1).ToList();
            var emp_data = _con.tbl_employee.FirstOrDefault(a => a.id == task_datas.employee_ids);
            mainModel _task_data = new mainModel {
                project_details = project,
               employee_details=_emp,
               employee_data=emp_data,
                task_statusdetails=_taskstatus,
                task_statusdata=status,
                project_data=project_info,
                risk_details=risk,
                task_typedata=_typedata,
                task_typedetails=type,
                task_prioritesData=_priority_selected,
                task_prioritesDetails=_priority,
              
            task_data = _con.tbl_tasks.Find(id),

            };

            return View(_task_data);
        }
        [HttpPost]
        public IActionResult update_tasks(task _task, IFormFile document)
        {
            if (document != null) // Check if a file was actually uploaded
            {
                string file_extension = Path.GetExtension(document.FileName);
                if (file_extension == ".pdf" || file_extension == ".docx" || file_extension == ".txt")
                {
                    Random ran = new Random();
                    string fileNAme = Path.GetFileName(document.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = fileNAme = R_num; // This line seems redundant; you're re-assigning fileNAme
                    string file_name = name + file_extension;
                    string filepath = Path.Combine(_evn.WebRootPath, "documents", file_name);
                    using (FileStream fs = new FileStream(filepath, FileMode.Create)) // Use 'using' for proper disposal
                    {
                        document.CopyTo(fs);
                    }

                    _task.document = file_name;
                    // Store the path in the project object
                }
                else
                {
                    TempData["msg1"] = "This type of file is not supported !";
                    return RedirectToAction("task_details");
                }
            }
            // If document is null, the code will skip the file processing block.  pro object will be saved without file info.
            _task.created_date = DateTime.Now.ToString();
            _task.updated_date = DateTime.Now.ToString();
            _con.tbl_tasks.Update(_task);
            _con.SaveChanges();
            TempData["msg1"] = "The Task is successfully updated now !";
            return RedirectToAction("task_details");
        }
        // add_file
        [HttpPost]
        public IActionResult add_file(int id,task_files _file,IFormFile file_name)
        {
            string file_extension = Path.GetExtension(file_name.FileName);
            if (file_extension == ".pdf" || file_extension == ".docx" || file_extension == ".txt")
            {
                Random ran = new Random();
                string fileNAme = Path.GetFileName(file_name.FileName);
                var R_num = ran.Next(0000, 9999).ToString();
                var name = fileNAme = R_num; // This line seems redundant; you're re-assigning fileNAme
                string file_names = name + file_extension;
                string filepath = Path.Combine(_evn.WebRootPath, "documents", file_names);
                using (FileStream fs = new FileStream(filepath, FileMode.Create)) // Use 'using' for proper disposal
                {
                    file_name.CopyTo(fs);
                }
            
                _file.date = DateTime.Now.Date.ToShortDateString();
              
                _file.file_name= file_names;
                _con.tbl_taks_file.Add(_file);
                _con.SaveChanges();
                TempData["msg8"] = "The file is successfully updated now !";
                return RedirectToAction("task_details");
            }
            else
            {
                TempData["msg8"] = "This type of file is not supported !";
                return RedirectToAction("task_details");
            }

           

        }
        public  IActionResult delete_file(int ids)
        {
            var del= _con.tbl_taks_file.Find(ids);
            _con.tbl_taks_file.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("view_task");
        }
        [HttpPost]
        public IActionResult add_subtask(sub_task _sub_task)
        {
            _con.tbl_subtask.Add(_sub_task);
            _con.SaveChanges();
            return RedirectToAction("view_task");
        }
        public IActionResult sub_task(int id)
        {
            var sub_task = _con.tbl_subtask.Where(p => p.task_id == id).Include(p=>p.emp_details).Include(p => p.risl_details).Include(p => p.task_details).ToList();
            if (sub_task.Count == 0)
            {
                TempData["msg"] = "There is not sub task ";
            }

            mainModel data = new mainModel
            {
                subTask_details = sub_task
            };
            return View(data);
        }
        public IActionResult delete_subtask(int id)
        {
            var del = _con.tbl_subtask.Find(id);
            _con.tbl_subtask.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("view_task");
        }
        public IActionResult update_subtasks(int id)
        {
            var sub_Taskdata = _con.tbl_subtask.Find(id);
            var emp = _con.tbl_employee.Where(p => p.status == 1).ToList();
            var risk = _con.tbl_risks.ToList();
            var _risk_data = _con.tbl_risks.FirstOrDefault(r => r.id == sub_Taskdata.risk_id);
            var emp_data = _con.tbl_employee.FirstOrDefault(r => r.id == sub_Taskdata.emp_id);

            mainModel data = new mainModel
            {employee_details=emp,
                risk_data = _risk_data,
                risk_details =risk,
                employee_data = emp_data,

                subTask_data =sub_Taskdata
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_subtasks(sub_task _task)
        {
            _con.tbl_subtask.Update(_task);
            _con.SaveChanges();
            return RedirectToAction("view_task");
        }
        public IActionResult maritialStatus_details()
        {
            var status = _con.tbl_maritalstatus.ToList();
            mainModel data = new mainModel { 
            materialStatus_details=status
            };
            return View(data);

        }
        [HttpPost]
        public IActionResult addmaritial_status(maritalStatus maritial_status)
        {
            _con.tbl_maritalstatus.Add(maritial_status);
            _con.SaveChanges();
            return RedirectToAction("maritialStatus_details");
        }
        public IActionResult delete_maritalStatus(int id)
        {
            var del= _con.tbl_maritalstatus.Find(id);
            _con.tbl_maritalstatus.Remove(del);

            _con.SaveChanges();
            return RedirectToAction("maritialStatus_details");
        }
        public IActionResult maritalStatusUpdate(int id)
        {
            var find_Id = _con.tbl_maritalstatus.Find(id);
            if (find_Id.status == 1)
            {
                find_Id.status = 0;
            }
            else {
                find_Id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("maritialStatus_details");
        }
        public IActionResult update_materialStatus(int id)
        {
            var find_id = _con.tbl_maritalstatus.Find(id);
           
            mainModel data = new mainModel
            {
              materialStatus_data=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_materialStatus(maritalStatus _status)
        {
            _con.tbl_maritalstatus.Update(_status);
            _con.SaveChanges();
            return RedirectToAction("maritialStatus_details");
        }
        public IActionResult employee_type()
        {
            var types = _con.tbl_emptypes.ToList();
            mainModel data = new mainModel
            {
                emp_types_details=types
            };
            return View(data);

        }

        public IActionResult delete_emptypes(int id)
        {
            var find_id = _con.tbl_emptypes.Find(id);
            _con.tbl_emptypes.Remove(find_id);
            _con.SaveChanges();
            return RedirectToAction("employee_type");
        }
        [HttpPost]
        public IActionResult addemployee_type(emp_type _empType)
        {
            _con.tbl_emptypes.Add(_empType);
            _con.SaveChanges();
            return RedirectToAction("employee_type");
        }
        public IActionResult employeeTypesstatusUpdate(int id)
        {
            var find_Id = _con.tbl_emptypes.Find(id);
            if (find_Id.status == 1)
            {
                find_Id.status = 0;
            }
            else
            {
                find_Id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("employee_type");
        }
        public IActionResult update_empType(int id)
        {
            var find_id = _con.tbl_emptypes.Find(id);

            mainModel data = new mainModel
            {
               emp_types_data=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_empType(emp_type _type)
        {
            _con.tbl_emptypes.Update(_type);
            _con.SaveChanges();
            return RedirectToAction("employee_type");
        }
       
       
        public IActionResult risk_typeDetails()
        {
            var _riskType = _con.tbl_risk_types.ToList();
            mainModel data = new mainModel
            {
                Risktypes_details=_riskType
            };
            if (_riskType.Count == 0)
            {
                TempData["msg"] = "There is no any Risk type";
            }

            return View(data);
        }
        public IActionResult add_riskType(risk_type _types)
        {
            _con.tbl_risk_types.Add(_types);
            _con.SaveChanges();
            return RedirectToAction("risk_typeDetails");
        }
        public IActionResult delete_riskType(int id)
        {
            var del = _con.tbl_risk_types.Find(id);
            _con.tbl_risk_types.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("risk_typeDetails");
        }
        public IActionResult risk_typestatusUpdate(int id)
        {
            var find_Id = _con.tbl_risk_types.Find(id);
            if (find_Id.status == 1)
            {
                find_Id.status = 0;
            }
            else
            {
                find_Id.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("risk_typeDetails");

        }
        public IActionResult update_risk_types(int id)
        {
            var find_id = _con.tbl_risk_types.Find(id);

            mainModel data = new mainModel
            {
               Risktypes_data=find_id
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult update_risk_types(risk_type _types)
        {
            _con.tbl_risk_types.Update(_types);
            _con.SaveChanges();
            return RedirectToAction("risk_typeDetails");
        }
    }
}

