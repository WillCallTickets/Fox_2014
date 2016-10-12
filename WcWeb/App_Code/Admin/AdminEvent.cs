using System;

/// <summary>
/// Summary description for AdminEvent
/// </summary>
namespace wctMain.Admin
{
    public class AdminEvent
    {
        public AdminEvent()
        {
        }

        //next, prev, first, last, page
        public class CollectionPagerEventArgs : EventArgs
        {
            protected int _pageSize;
            protected int _pageIndex;

            //Alt Constructor
            public CollectionPagerEventArgs(int newPageSize, int newPageIndex)
            {
                _pageSize = newPageSize;
                _pageIndex = newPageIndex;
            }

            public int NewPageSize { get { return _pageSize; } }
            public int NewPageIndex { get { return _pageIndex; } }
        }
        public delegate void CollectionPagerChangedEvent(object sender, wctMain.Admin.AdminEvent.CollectionPagerEventArgs e);
                
        public class ActiveEditPrincipalChangedEventArgs : EventArgs
        {
            protected Wcss._Enums.Principal _principal;
            public Wcss._Enums.Principal Principal { get { return _principal; } }

            //Alt Constructor
            public ActiveEditPrincipalChangedEventArgs(Wcss._Enums.Principal principal)
            {
                _principal = principal;
            }
        }


        public delegate void CurrentActiveEditPrincipalChangedEvent(object sender, AdminEvent.ActiveEditPrincipalChangedEventArgs e);
        public static event CurrentActiveEditPrincipalChangedEvent CurrentActiveEditPrincipalChanged;
        public static void OnCurrentActiveEditPrincipalChanged(object sender, Wcss._Enums.Principal newPrincipal)
        {
            if (CurrentActiveEditPrincipalChanged != null)
            {
                //ensures that the context variable is updated as well
                CurrentActiveEditPrincipalChanged(sender, new AdminEvent.ActiveEditPrincipalChangedEventArgs(newPrincipal));                
            }
        }

        public class EditorEntityChangedEventArgs : EventArgs
        {
            protected int _idx;
            protected string _name;

            //Alt Constructor
            public EditorEntityChangedEventArgs(int idx, string name)
            {
                _idx = idx;
                _name = name;
            }

            public int Idx { get { return _idx; } }
            public string Name { get { return _name; } }
        }

        public delegate void CurrentSubscriptionEmailChangedEvent(object sender, EventArgs e);
        public static event CurrentSubscriptionEmailChangedEvent CurrentSubscriptionEmailChanged;
        public static void OnCurrentSubscriptionEmailChanged(object sender)
        {
            if (CurrentSubscriptionEmailChanged != null)
            {
                ResetSubscriptionEmailSelectionCookies(sender);
                CurrentSubscriptionEmailChanged(sender, new EventArgs());
            }
        }
        public delegate void CurrentMailerChangedEvent(object sender, EventArgs e);
        public static event CurrentMailerChangedEvent CurrentMailerChanged;
        public static void OnCurrentMailerChanged(object sender)
        {
            if (CurrentMailerChanged != null)
            {
                CurrentMailerChanged(sender, new EventArgs());
            }
        }
        public delegate void CurrentMailerTemplateChangedEvent(object sender, EventArgs e);
        public static event CurrentMailerTemplateChangedEvent CurrentMailerTemplateChanged;
        public static void OnCurrentMailerTemplateChanged(object sender)
        {
            if (CurrentMailerTemplateChanged != null)
            {
                //ensures that the context variable is updated as well
                CurrentMailerTemplateChanged(sender, new EventArgs());
            }
        }

        protected static void ResetSubscriptionEmailSelectionCookies(object sender)
        {
            wctMain.Controller.MainBaseControl ctrl  = sender as wctMain.Controller.MainBaseControl;

            if (ctrl != null)
            {
                ctrl.Atx.ActiveMailerReviewTab = string.Empty;
                ctrl.Atx.ActiveMailerSendTab = string.Empty;
            }
        }




        public delegate void MailerContent_ContentChangedEvent(object sender, EventArgs e);
        public static event MailerContent_ContentChangedEvent MailerContent_ContentChanged;
        public static void OnMailerContent_ContentChanged(object sender)
        {
            if (MailerContent_ContentChanged != null)
            {
                MailerContent_ContentChanged(sender, new EventArgs());
            }
        }

        public class ShowChosenEventArgs : EventArgs
        {
            protected int _idx;

            //Default Constructor
            public ShowChosenEventArgs()
            {
                _idx = 0;
            }

            //Alt Constructor
            public ShowChosenEventArgs(int idx)
            {
                _idx = idx;
            }

            public int ChosenId { get { return _idx; } }
        }
        public delegate void ShowChosenEvent(object sender, AdminEvent.ShowChosenEventArgs e);
        public static event ShowChosenEvent ShowChosen;
        public static void OnShowChosen(object sender, int idx)
        {
            if (ShowChosen != null)
            {
                //ensures that the context variable is updated as well
                BindNewShowSelectionToContext(idx);
                ShowChosen(sender, new AdminEvent.ShowChosenEventArgs(idx));
            }
        }

        private static void BindNewShowSelectionToContext(int idx)
        {
            AdminContext atx = new AdminContext();

            atx.SetCurrentShowRecord(idx);
        }

        public class ActChosenEventArgs : EventArgs
        {
            protected int _idx;
            protected string _name;

            //Default Constructor
            public ActChosenEventArgs() 
            {
                _idx = 0;
                _name = string.Empty;
            }

            //Alt Constructor
            public ActChosenEventArgs(int idx, string name)
            {
                _idx = idx;
                _name = name;
            }

            public int ChosenId { get { return _idx; } }
            public string ChosenName { get { return _name; } }
        }
        public delegate void ActChosenEvent(object sender, ActChosenEventArgs e);
        public static event ActChosenEvent ActChosen;
        public static void OnActChosen(object sender, int id, string name)
        {
            if (ActChosen != null) { ActChosen(sender, new ActChosenEventArgs(id, name)); }
        }

        public class UpdateParentGridEventArgs : EventArgs
        {
            protected bool _isInsert = false;

            //Default Constructor
            public UpdateParentGridEventArgs()
            {
                _isInsert = false;
            }

            //Alt Constructor
            public UpdateParentGridEventArgs(bool IsCompletedInsert)
            {
                _isInsert = IsCompletedInsert;
            }

            public bool IsCompletedInsert { get { return _isInsert; } }
        }
        public delegate void UpdateParentGridEvent(object sender, UpdateParentGridEventArgs e);
        public static event UpdateParentGridEvent UpdateParentGrid;
        public static void OnUpdateParentGrid(object sender, bool isCompletedInsert)
        {
            if (UpdateParentGrid != null) { UpdateParentGrid(sender, new UpdateParentGridEventArgs(isCompletedInsert)); }
        }

        public class GridSelectionChangedEventArgs : EventArgs
        {
            protected int _selectedId = 0;

            //Default Constructor
            public GridSelectionChangedEventArgs()
            {
            }

            //Alt Constructor
            public GridSelectionChangedEventArgs(int selectedId)
            {
                _selectedId = selectedId;
            }

            public int SelectedId { get { return _selectedId; } }
        }
        public delegate void GridSelectionChangedEvent(object sender, GridSelectionChangedEventArgs e);
        public static event GridSelectionChangedEvent GridSelectionChanged;
        public static void OnGridSelectionChanged(object sender, int SelectedId)
        {
            if (GridSelectionChanged != null) { GridSelectionChanged(sender, new GridSelectionChangedEventArgs(SelectedId)); }
        }

        public delegate void UpdateSalePromotionEvent(object sender, EventArgs e);
        public static event UpdateSalePromotionEvent UpdateSalePromotion;
        public static void OnUpdateSalePromotion(object sender)
        {
            if (UpdateSalePromotion != null) { UpdateSalePromotion(sender, new EventArgs()); }
        }
        public delegate void ShowNameChangeEvent(object sender, EventArgs e);
        public static event ShowNameChangeEvent ShowNameChanged;
        public static void OnShowNameChanged(object sender)
        {
            if (ShowNameChanged != null) { ShowNameChanged(sender, new EventArgs()); }
        }

        public delegate void ShowListStartDateEvent(object sender, EventArgs e);
        public static event ShowListStartDateEvent ShowListStartDateChanged;
        public static void OnShowListStartDateChanged(object sender)
        {
            if (ShowListStartDateChanged != null) { ShowListStartDateChanged(sender, new EventArgs()); }
        }

        public delegate void CurrentShowEditContextChangeEvent(object sender, EventArgs e);
        public static event CurrentShowEditContextChangeEvent CurrentShowEditContextChanged;
        public static void OnCurrentShowEditContextChanged(object sender)
        {
            if (CurrentShowEditContextChanged != null) { CurrentShowEditContextChanged(sender, new EventArgs()); }
        }
    }
}
