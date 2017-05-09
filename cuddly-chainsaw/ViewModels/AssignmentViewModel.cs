using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cuddly_chainsaw.Models;
using Windows.UI.Popups;
using Windows.Storage;
using System.Threading;

/***************************************
 * get 所有作业列表
 * get 未完成作业列表
 * get 完成作业列表
 * set 提交作业
 * set Admin修改当前作业信息
 * set Admin发布作业
 * set Admin删除作业
 * allAssignments： 所有作业
 * selectedAssignment：当前在操作的作业
 * **************************************/
namespace cuddly_chainsaw.ViewModels
{
    class AssignmentViewModel
    {
        //所有作业列表
        private ObservableCollection<Models.Assignment> allAssignments = new ObservableCollection<Models.Assignment>();
        public ObservableCollection<Models.Assignment> AllAssignments { get { return this.allAssignments; } }

        //已经完成作业列表
        private ObservableCollection<Models.Assignment> doneAssignments = new ObservableCollection<Models.Assignment>();
        public ObservableCollection<Models.Assignment> DoneAssignments { get { return this.doneAssignments; } }

        //进行中作业列表
        private ObservableCollection<Models.Assignment> doingAssignments = new ObservableCollection<Models.Assignment>();
        public ObservableCollection<Models.Assignment> DoingAssignments { get { return this.doingAssignments; } }

        //当前在操作的作业
        private Models.Assignment selectedAssignment = default(Models.Assignment);
        public Models.Assignment SelectedAssignment { get { return selectedAssignment; } set { this.selectedAssignment = value; } }


        public AssignmentViewModel()
        {
            InitializeAllAssignments();
        }

        //获取所有作业的信息，初始化allAssignments, doingAssignments, doineAssignments
        public async void InitializeAllAssignments()
        {
            await getAllAssignments();

        }
        
        //获取所有的作业
        public async Task getAllAssignments()
        {
            ResponseError error = null;
            List<Assignment> allAsg = null;
            string message = "";
            Action<ResponseError, List<Assignment>, string> cb = delegate (ResponseError e, List<Assignment> all, string msg)
            {
                error = e;
                allAsg = all;
                message = msg;
                if (error != null)
                {
                    var i = new MessageDialog(message).ShowAsync();
                }
                else
                {
                    foreach (Assignment a in allAsg)
                    {
                        allAssignments.Add(a);
                    }
                    getDoingAssignments();
                    getDoneAssignments();
                }
                selectedAssignment = null;
            };
            await Assignment.getAll(cb);
        }
        
        //已完成的作业
        public void getDoneAssignments()
        {
            foreach (Assignment a in allAssignments)
            {
                if (a.isEnded())
                {
                    doneAssignments.Add(a);
                }
            }
        }
        
        //未完成的作业
        public void getDoingAssignments()
        {
            foreach (Assignment a in allAssignments)
            {
                if (!a.isEnded())
                {
                    doingAssignments.Add(a);
                }
            }
        }
        
        //提交作业
        public async Task<Boolean> submitAssignments(StorageFile file)
        {
            Assignment temp = selectedAssignment;
            ResponseError error = null;
            string data = "";
            string message = "";
            Action<ResponseError, string, string> cb = delegate (ResponseError e, string d, string msg) {
                error = e;
                data = d;
                message = msg;
            };
            await temp.submit(file, cb);
            if (error != null)
            {
                var i = new MessageDialog(message).ShowAsync();
                return false;
            }
            return true;
        }
        
        //新建作业
        public async Task<Boolean> newAssignments(Assignment temp)
        {
            selectedAssignment = temp;
            ResponseError error = null;
            string message = "";
            if (temp == null) return false;
            Action<ResponseError, Assignment, string> cb = delegate (ResponseError e, Assignment t, string msg) {
                error = e;
                message = msg;
                temp = t;
                if (error != null)
                {
                    var i = new MessageDialog(message).ShowAsync();
                }
                else
                {
                    allAssignments.Clear();
                    doingAssignments.Clear();
                    doneAssignments.Clear();
                    //从本地中添加新的作业, 需要把数据库全面更新，因为没法从本地直接添加id
                }
            };
            await selectedAssignment.save(cb);
            await getAllAssignments();
            SelectedAssignment = temp;
            return false;
        }
        
        //修改当前作业
        public async Task<Boolean> updateAssignments()
        {
            Assignment temp = selectedAssignment;
            ResponseError error = null;
            string message = "";
            if (temp == null) return false;
            Action<ResponseError, Assignment, string> cb = delegate (ResponseError e, Assignment t, string msg)
            {
                error = e;
                message = msg;
                temp = t;
                if (error != null)
                {
                    var i = new MessageDialog(message).ShowAsync();
                }
                else
                {
                    //从本地中修改当前作业
                    allAssignments.Remove(selectedAssignment);
                    allAssignments.Add(temp);
                    if (doingAssignments.IndexOf(selectedAssignment) != -1)
                    {
                        doingAssignments.Remove(selectedAssignment);
                        doingAssignments.Add(temp);
                    }
                    if (doneAssignments.IndexOf(selectedAssignment) != -1)
                    {
                        doneAssignments.Remove(selectedAssignment);
                        doneAssignments.Add(temp);
                    }
                    selectedAssignment = temp;
                }
            };
            await temp.update(cb);
            return false;
        }
        
        //删除当前作业
        public async Task<Boolean> deleteAssignments()
        {
            ResponseError error = null;
            Assignment asg = selectedAssignment;
            string message = "";
            string aid = "";
            if (asg == null)
            {
                return false;
            }
            aid = asg.getAssignmentId();
            Action<ResponseError, Assignment, string> cb = delegate (ResponseError e, Assignment a, string msg)
            {
                error = e;
                asg = a;
                message = msg;
                if (error != null)
                {
                    var i = new MessageDialog(message).ShowAsync();
                }
                else
                {
                    //从本地中删除当前作业
                    allAssignments.Remove(selectedAssignment);
                    if (doingAssignments.IndexOf(selectedAssignment) != -1)
                    {
                        doingAssignments.Remove(selectedAssignment);
                    }
                    if (doneAssignments.IndexOf(selectedAssignment) != -1)
                    {
                        doneAssignments.Remove(selectedAssignment);
                    }
                    selectedAssignment = null;
                }
            };
            await Assignment.delete(aid, cb);
            return false;
        }
    }
}
