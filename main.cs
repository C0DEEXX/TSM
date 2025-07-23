using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Threading.Tasks;
using static Team;
using System.Xml.Linq;


[Serializable]
public class User
{
    private string pass;
    public string username;
    private Task currentTask;
    public static int usercounter = 0;
    public List<Task> numbers = new List<Task>();
    public static int TeamMemberID = 0;
    public List<User> users = new List<User>();
    public bool y = false;

    public User(string u, string p)
    {
        username = u;
        pass = p;
        usercounter++;
        TeamMemberID++;
    }
    public void sign_inUser(string u, string p)
    {
        users = LoadUsers();

        try
        {
            User storedUser = users.Find(user => user.username == u && user.verify_pass(p));

            if (storedUser != null)
            {
                Console.WriteLine("Login successful!");
                y = true;
            }
            else
            {
                Console.WriteLine("Incorrect username or password. Login failed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred during login: " + ex.Message);
        }
    }


    public void sign_upUser(string u, string p)
    {
        users = LoadUsers();

        if (users.Any(user => user.username == u))
        {
            Console.WriteLine("Username already exists. Please choose a different username.");
        }
        else
        {
            users.Add(new User(u, p));
            SaveUsers(users);
            Console.WriteLine("User account created successfully!");
        }
    }

    private void SaveUsers(List<User> users)
    {
        using (var fs = new FileStream("data.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, users);
        }
    }


    public List<User> LoadUsers()
    {
        List<User> users = new List<User>();

        if (File.Exists("data.txt"))
        {
            using (var fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    users = (List<User>)bf.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during deserialization: " + ex.Message);
                }
            }
        }

        return users;
    }


    public bool verify_pass(string enteredPassword)
    {
        return enteredPassword == pass;
    }
    public void printU()
    {
        foreach (User n in users)
        {
            Console.WriteLine(n.username);
        }
    }
    public void print4()
    {


        for (int i = 0; i < numbers.Count; i++)
        {

            Console.WriteLine(numbers[i].getTitle());


        }

    }
    public void TeamMemberTaskView()
    {
        users = LoadUsers();
        if (y == true)
        {
            Console.WriteLine("Tasks assigned to you:");
            print34();
        }
        else
            Console.WriteLine("No tasks assigend to this user");
    }
    public void TeamMemberTaskUpdate()
    {
        users = LoadUsers();
        if (y == true)
        {
            Console.WriteLine("Tasks assigned to you:");
            print34();
            Console.WriteLine("Enter the task ID you would like to update:");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice > 0)
            {
                Console.WriteLine("Enter the new task:");
                Console.WriteLine("Enter your new title:");
                string e = Console.ReadLine();
                Console.WriteLine("Enter your new description:");
                string v = Console.ReadLine();
                Console.WriteLine("Enter your new due date:");
                string w = Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }
    }
    public void SaveTasks(Task number)
    {
        List<Task> numbers = new List<Task>();
        if (File.Exists("TaskData.txt"))
        {
            using (FileStream fs = new FileStream("TaskData.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    numbers = (List<Task>)bf.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during task deserialization: " + ex.Message);
                }
            }
        }
        numbers.Add(number);

        using (var fsw = new FileStream("Taskdata.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bfw = new BinaryFormatter();
            bfw.Serialize(fsw, numbers);
        }
    }
    public List<Task> LoadTasksnumbers()
    {
        List<Task> numbers = new List<Task>();
        if (File.Exists("TaskData.txt"))
        {
            using (FileStream fsw = new FileStream("TaskData.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bfw = new BinaryFormatter();
                try
                {
                    numbers = (List<Task>)bfw.Deserialize(fsw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during task deserialization: " + ex.Message);
                }
            }
        }
        return numbers;

    }
    public void print34()
    {
        List<Task> numbers = LoadTasksnumbers();
        for (int i = 0; i < numbers.Count; i++)
        {
            Console.WriteLine(numbers[i].getTitle());
            Console.WriteLine(numbers[i].getTaskDescription());
            Console.WriteLine(numbers[i].getDueDate());
            Console.WriteLine(numbers[i].getTaskID());

        }
    }
}

[Serializable]
public class Task
{
    int userID;
    static int taskCnt = 0;
    int taskID;
    int stateCnt;  // stateCnt = 1 (To Do) , stateCnt = 2 (Pending) , stateCnt = 3 (Done)
    string taskDescription;
    string taskTitle;
    string dueDate;

    public Task()
    {
    }

    public Task(string title, string taskDesc, string dueDATE)
    {

        this.taskTitle = title;
        this.taskDescription = taskDesc;
        this.dueDate = dueDATE;


        stateCnt = 1;
        taskCnt++;
        taskID = taskCnt;

        saveTaskData(this);
    }

    public string getTitle()
    {
        return this.taskTitle;
    }

    public string getTaskDescription()
    {
        return this.taskDescription;
    }

    public string getDueDate()
    {
        return this.dueDate;
    }

    public int getTaskID()
    {
        return this.taskID++;
    }

    public int getStateCnt()
    {
        return this.stateCnt;
    }

    public void updateStatus()
    {

        if (stateCnt < 3) stateCnt++;
        else Console.WriteLine("The task is already finished.\n");
    }



    public void trackTaskProgress()
    {

        switch (this.stateCnt)
        {
            case 1: Console.WriteLine("The task is currently at: To Do\n"); break;
            case 2: Console.WriteLine("The task is currently at: Pending\n"); break;
            case 3: Console.WriteLine("The task is currently at: Done\n"); break;
        }

    }

    public void print()
    {
        Console.WriteLine("Task information:\n" +
            "Task Title: {0}\n" +
            "Task description: {1}\n" +
            "The task is due to : {2}\n" +
            "Task ID: {3}", this.taskTitle, this.taskDescription, this.dueDate, this.taskID);

    }

    public void saveTaskData(Task t)
    {
        List<Task> tasks = new List<Task>();
        if (File.Exists("TaskData.txt"))
        {
            using (FileStream fs = new FileStream("TaskData.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    tasks = (List<Task>)bf.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during task deserialization: " + ex.Message);
                }
            }
        }

        tasks.Add(t);

        using (FileStream fs = new FileStream("TaskData.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, tasks);
        }
    }


}

[Serializable]
public class Team
{
    public string TeamName;
    public string TeamID;


    public Team(string name, string ID)
    {
        TeamName = name;
        TeamID = ID;

    }

}
[Serializable]
public class Manager : User
{
    public List<Team> teams = new List<Team>();
    public List<Manager> managers = new List<Manager>();
    public List<Task> retrievedTask = new List<Task>();
    List<Task> Reports = new List<Task>();
    public bool x = false;
    public Manager(string u, string p) : base(u, p)
    {
    }
    public void sign_inManager(string u, string p)
    {
        managers = LoadManager();

        try
        {
            Manager storedManager = managers.Find(manager => manager.username == u && manager.verify_pass(p));

            if (storedManager != null)
            {
                Console.WriteLine("Login successful!");
                x = true;
            }
            else
            {
                Console.WriteLine("Incorrect username or password. Login failed.");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred during login: " + ex.Message);
        }
    }

    public void sign_upManager(string u, string p)
    {
        managers = LoadManager();

        if (managers.Any(manager => manager.username == u))
        {
            Console.WriteLine("Username already exists. Please choose a different username.");
        }
        else
        {
            managers.Add(new Manager(u, p));
            SaveManager(managers);
            Console.WriteLine("Manager account created successfully!");
        }
    }

    private void SaveManager(List<Manager> managers)
    {
        using (var fss = new FileStream("dataManager.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bff = new BinaryFormatter();
            bff.Serialize(fss, managers);
        }
    }

    private List<Manager> LoadManager()
    {
        List<Manager> managers = new List<Manager>();

        if (File.Exists("dataManager.txt"))
        {
            using (var fss = new FileStream("dataManager.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bff = new BinaryFormatter();
                try
                {
                    managers = (List<Manager>)bff.Deserialize(fss);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during deserialization: " + ex.Message);
                }
            }
        }

        return managers;
    }
    private List<Task> LoadTasks()
    {
        List<Task> retrievedTask = new List<Task>();
        if (File.Exists("TaskData.txt"))
        {
            using (FileStream fsw = new FileStream("TaskData.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bfw = new BinaryFormatter();
                try
                {
                    retrievedTask = (List<Task>)bfw.Deserialize(fsw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during task deserialization: " + ex.Message);
                }
            }
        }
        return retrievedTask;

    }
    public void SaveTasks(List<Task> numbers)
    {
        using (var fsw = new FileStream("TaskData.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bfw = new BinaryFormatter();
            bfw.Serialize(fsw, numbers);
        }

    }
    public void print2()
    {
        List<User> users = new List<User>();
        if (File.Exists("data.txt"))
        {
            using (var fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    users = (List<User>)bf.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during deserialization: " + ex.Message);
                }
            }
            for (int i = 0; i < users.Count; i++)
            {

                Console.WriteLine(users[i].username);


            }
        }
    }
    public void print3()
    {
        List<Task> retrievedTask = LoadTasks();

        for (int i = 0; i < retrievedTask.Count; i++)
        {
           // if (retrievedTask[i].getStateCnt() == 1)
            //{
                Console.WriteLine(retrievedTask[i].getTitle());

            //}
        }

    }
    private List<Team> LoadTeam()
    {
        List<Team> teams = new List<Team>();

        if (File.Exists("Team.txt"))
        {
            using (var fsr = new FileStream("Team.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bfr = new BinaryFormatter();
                try
                {
                    teams = (List<Team>)bfr.Deserialize(fsr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No team with this name was found, creating new team.... " + ex.Message);

                }
            }
        }
        return teams;
    }
    public void SaveTeams(List<Team> teams)
    {
        using (var fsr = new FileStream("Team.txt", FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter bfr = new BinaryFormatter();
            bfr.Serialize(fsr, teams);
        }

    }

    public void CreateTeam(string name, string ID)
    {
        List<Team> teams = LoadTeam();

        if (teams.Any(team => team.TeamName == name))
        {
            Console.WriteLine("Team name already exists. Please choose a different Team-Name.");
        }
        else
        {
            teams.Add(new Team(name, ID));
            SaveTeams(teams);
            Console.WriteLine("Team '{0}' created with ID '{1}'.", name, ID);

        }

    }


    public void CreateTask()
    {
        Console.WriteLine("Enter the task title:");
        string title = Console.ReadLine();
        Console.WriteLine("Enter the task description:");
        string description = Console.ReadLine();
        Console.WriteLine("Enter the task due date:");
        string dueDate = Console.ReadLine();

        Task newTask = new Task(title, description, dueDate);
        newTask.print();  // Display task information
        newTask.trackTaskProgress();
        newTask.saveTaskData(newTask);  // Save the task to a file
    }
    public void AssignUserToTeam(string username, string teamname)
    {
        List<Team> teams = LoadTeam();
        Team team = teams.Find(u => u.TeamName == teamname);


        if (team != null)
        {
            Console.WriteLine("User '{0}' assigned to Team '{1}'.", username, team.TeamName);
        }
        else
        {
            Console.WriteLine("Team with name '{0}' not found.", teamname);
        }
    }

    public void AssignTaskToTeamMember(string nameOfUser, string titleOfTask)
    {
        retrievedTask = LoadTasks();

        Task taskToAssign = retrievedTask.Find(task => task.getTitle() == titleOfTask && task.getStateCnt() == 1);

        if (taskToAssign != null)
        {
            users = LoadUsers();
            User userToAssign = users.Find(user => user.username == nameOfUser);


            if (userToAssign != null)
            {

                taskToAssign.updateStatus();
                taskToAssign.getTaskID();
                userToAssign.SaveTasks(taskToAssign);

                Console.WriteLine("Task '{0}' assigned to '{1}' and set to Pending.", titleOfTask, nameOfUser);
            }
            else
            {
                Console.WriteLine("User '{0}' not found.", nameOfUser);
            }
        }
        else
        {
            Console.WriteLine("Task '{0}' not found or is not in To Do status.", titleOfTask);
        }

    }
    public void GenerateTaskReport()
    {

        List<Task> retrievedTask = LoadTasks();

        for (int i = 0; i < retrievedTask.Count; i++)
        {


            Console.WriteLine(retrievedTask[i].getTitle());
            Console.WriteLine(retrievedTask[i].getTaskDescription());
            Console.WriteLine(retrievedTask[i].getDueDate());
            Console.WriteLine(retrievedTask[i].getTaskID());
            switch (retrievedTask[i].getStateCnt())
            {

                case 1:
                    Console.WriteLine("Task Status: To Do (0%)");
                    break;
                case 2:
                    Console.WriteLine("Task Status: Pending (50%)");
                    break;
                case 3:
                    Console.WriteLine("Task Status: Done (100%)");
                    break;
                default:
                    Console.WriteLine("Invalid Task Status");
                    break;
            }
            Console.WriteLine("..........................................................................................");
        }
    }

}






interface IUserInterface
{
    void ShowOptions();
    void ShowOptions2();
    void ShowOptions3();
}

class ConcreteInterface : IUserInterface
{
    public void ShowOptions()
    {
        Console.WriteLine("[1] Sign Up As Manager");
        Console.WriteLine("[2] Sign Up As User (Team Member)");
        Console.WriteLine("[3] Sign In As Manager");
        Console.WriteLine("[4] Sign In As User (Team Member)");
        Console.WriteLine("[5] EXIT");

    }
    public void ShowOptions2()
    {
        Console.WriteLine("[1] Create New Task");
        Console.WriteLine("[2] Create New Team And Assign User To Team");
        Console.WriteLine("[3] Assign Task To Team Member");
        Console.WriteLine("[4] Generate Task Report");
        Console.WriteLine("[5] Sign Out");
    }
    public void ShowOptions3()
    {
        Console.WriteLine("[1] Track Task Progress (Update Task)");
        Console.WriteLine("[2] View Task Details");
        Console.WriteLine("[3] Sign Out");
    }
}
class EXIT
{
    public static void TerminateProgram()
    {
        Console.WriteLine("Exiting the program. Goodbye!");
        Environment.Exit(0);
    }
}

class Program
{
    static void Main()
    {
        ConcreteInterface myInterface = new ConcreteInterface();
        bool exitProgram = false;
      
        
            myInterface.ShowOptions();

            Console.Write("Choose an option: ");
            int chosenOption;
            int chosennumber;
            int chosennumber2;
            if (int.TryParse(Console.ReadLine(), out chosenOption))

            {
                if (chosenOption == 1)
                {
                    Console.WriteLine("Enter Username:");
                    string u = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string p = Console.ReadLine();
                    Manager manager1 = new Manager(u, p);
                    manager1.sign_upManager(u, p);
                }
                else if (chosenOption == 2)
                {
                    Console.WriteLine("Enter Username:");
                    string u = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string p = Console.ReadLine();
                    User user1 = new User(u, p);
                    user1.sign_upUser(u, p);
                }
                else if (chosenOption == 3)
                {
                    Console.WriteLine("Enter Username:");
                    string u = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string p = Console.ReadLine();
                    Manager manager1 = new Manager(u, p);
                    manager1.sign_inManager(u, p);
                    if (manager1.x)
                    {
                        
                        myInterface.ShowOptions2();
                       
                    
                    }
                    else
                    {

                    }

                    if (int.TryParse(Console.ReadLine(), out chosennumber))
                    {
                        if (chosennumber == 1)
                        {
                            manager1.CreateTask();
                        if (manager1.x && chosennumber == 1)
                        {

                            while (manager1.x && chosennumber == 1)
                            {
                                myInterface.ShowOptions2();
                                break;
                            }


                        }
                    }
                        else if (chosennumber == 2)
                        {
                            Console.WriteLine("Enter the team name:");
                            string teamname = Console.ReadLine();
                            Console.WriteLine("Enter the team ID:");
                            string teamid = Console.ReadLine();
                            manager1.CreateTeam(teamname, teamid);
                            Console.WriteLine("Choose from the following users:");
                            manager1.print2();
                            Console.WriteLine("Enter the name from the list:");
                            string name = Console.ReadLine();
                            manager1.AssignUserToTeam(name, teamname);
                        if (manager1.x && chosennumber == 2)
                        {
                            while (manager1.x && chosennumber == 2)
                            {
                                myInterface.ShowOptions2();
                                break;
                            }

                        }
                    }
                        else if (chosennumber == 3)
                        {
                            manager1.print3();
                            Console.WriteLine("Enter the title of the task from the following list:");
                            string titleOfTask = Console.ReadLine();
                            manager1.print2();
                            Console.WriteLine("Enter the name of the user from the following list:");
                            string nameOfUser = Console.ReadLine();
                            manager1.AssignTaskToTeamMember(nameOfUser, titleOfTask);
                        if (manager1.x && chosennumber == 3)
                        {
                            while (manager1.x && chosennumber == 3)
                            {
                                myInterface.ShowOptions2();
                                break;
                            }

                        }
                    }
                        else if (chosennumber == 4)
                        {
                            manager1.GenerateTaskReport();
                        if (manager1.x && chosennumber == 4)
                        {

                            while (manager1.x && chosennumber == 4)
                            {
                                myInterface.ShowOptions2();
                                break;
                            }


                        }
                    }
                        else if (chosennumber == 5)
                        {
                        myInterface.ShowOptions();
                    }


                    }

                }
                else if (chosenOption == 4)
                {
                    Console.WriteLine("Enter Username:");
                    string u = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string p = Console.ReadLine();
                    User user1 = new User(u, p);
                    user1.sign_inUser(u, p);
                    if (user1.y)
                    {
                        myInterface.ShowOptions3();
                    }
                    else
                    {

                    }

                    if (int.TryParse(Console.ReadLine(), out chosennumber2))
                    {
                        if (chosennumber2 == 1)
                        {

                            user1.TeamMemberTaskUpdate();
                        if (user1.y && chosennumber2 == 1)
                        {
                            myInterface.ShowOptions3();
                        }
                    }
                        else if (chosennumber2 == 2)
                        {


                            user1.TeamMemberTaskView();
                        if (user1.y && chosennumber2 == 2)
                        {
                            myInterface.ShowOptions3();
                        }
                    }
                        else if (chosennumber2 == 3)
                        {
                        myInterface.ShowOptions();
                    }

                    }


                }
                else if (chosenOption == 5)
                {
                    EXIT.TerminateProgram();
                }


            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

