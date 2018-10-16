using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoloNimbus.Module.Web
{
    [PropertyEditor(typeof(BaseObject), true)]
    public class ASPxLookupPropertyEditorWithEdit : ASPxLookupPropertyEditor
    {
        public ASPxLookupPropertyEditorWithEdit(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        PopupWindowShowAction editObjectAction;

        protected override WebControl CreateEditModeControlCore()
        {
            WebControl control = base.CreateEditModeControlCore();
            ASPxButtonEditBase buttonEdit;
            if (UseFindEdit())
            {
                buttonEdit = FindEdit.Editor;
            }
            else
            {
                buttonEdit = DropDownEdit.DropDown;
            }
            if (editObjectAction == null)
            {
                editObjectAction = new PopupWindowShowAction(null, MemberInfo.Name + "_ASPxLookupEditor_EditObject", PredefinedCategory.Unspecified);
                editObjectAction.CustomizePopupWindowParams += editObjectAction_CustomizePopupWindowParams;
                editObjectAction.Application = application;
            }
            EditButton editButton = new EditButton();
            ASPxImageHelper.SetImageProperties(editButton.Image, "Editor_Edit", 16, 16);
            buttonEdit.Buttons.Add(editButton);
            buttonEdit.Load += new EventHandler(buttonEdit_Load);
            return control;
        }

        void buttonEdit_Load(object sender, EventArgs e)
        {
            ASPxButtonEditBase buttonEdit = (ASPxButtonEditBase)sender;
            string showModalWindowScript = application.PopupWindowManager.GetShowPopupWindowScript(editObjectAction, null, buttonEdit.ClientID, false, editObjectAction.IsSizeable, false, false);
            ButtonEditClientSideEventsBase clientSideEvents = (ButtonEditClientSideEventsBase)buttonEdit.GetClientSideEvents();
            int index = clientSideEvents.ButtonClick.LastIndexOf("}");
            string script = String.Format("if(e.buttonIndex == 2 && s.GetText() != '{0}') {{ {1} e.handled = true; e.processOnServer = false; }}", CaptionHelper.NullValueText, showModalWindowScript);
            clientSideEvents.ButtonClick = clientSideEvents.ButtonClick.Insert(index, script);
        }

        void editObjectAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace editObjectSpace = objectSpace.CreateNestedObjectSpace();
            DetailView view = application.CreateDetailView(editObjectSpace, editObjectSpace.GetObject(GetControlValueCore()));
            view.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = view;
            e.DialogController.SaveOnAccept = true;
        }
    }
}

