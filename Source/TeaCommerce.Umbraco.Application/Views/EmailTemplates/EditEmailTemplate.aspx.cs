using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Views.Shared.Partials;
using umbraco.cms.businesslogic.language;
using umbraco.uicontrols;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.EmailTemplates {
  public partial class EditEmailTemplate : UmbracoProtectedPage {

    private EmailTemplate emailTemplate = EmailTemplateService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
    private IEnumerable<Language> umbracoLanguages = Language.GetAllAsList();

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, emailTemplate.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );
      PPnlName.Text = CommonTerms.Name;
      PPnlAlias.Text = CommonTerms.Alias;
      PPnlShouldClientReceiveEmail.Text = CommonTerms.SendEmailToCustomer;

      PnDefaultSettings.Text = CommonTerms.DefaultSettings;
      PPnlSubject.Text = CommonTerms.Subject;
      PPnlSenderName.Text = CommonTerms.SenderName;
      PPnlSenderAddress.Text = CommonTerms.SenderAddress;
      PPnlToAddresses.Text = CommonTerms.ToAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      PPnlCCAddresses.Text = CommonTerms.CCAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      PPnlBCCAddresses.Text = CommonTerms.BCCAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      PPnlTemplate.Text = CommonTerms.TemplateFile;

      foreach ( Language language in umbracoLanguages )
        AddTab( language.CultureAlias, AddLanguagePane( language.id ), SaveButton_Clicked );
    }

    private Pane AddLanguagePane( int? languageId ) {
      string strLanguageId = languageId != null ? languageId.Value.ToString() : string.Empty;

      Pane pane = new Pane();

      PropertyPanel ppnlSubject = new PropertyPanel();
      ppnlSubject.Text = CommonTerms.Subject;
      pane.Controls.Add( ppnlSubject );

      TextBox txtSubject = new TextBox();
      txtSubject.ID = "TxtSubject" + strLanguageId;
      txtSubject.CssClass = "guiInputText guiInputStandardSize";
      ppnlSubject.Controls.Add( txtSubject );

      PropertyPanel ppnlSenderName = new PropertyPanel();
      ppnlSenderName.Text = CommonTerms.SenderName;
      pane.Controls.Add( ppnlSenderName );

      TextBox txtSenderName = new TextBox();
      txtSenderName.ID = "TxtSenderName" + strLanguageId;
      txtSenderName.CssClass = "guiInputText guiInputStandardSize";
      ppnlSenderName.Controls.Add( txtSenderName );

      PropertyPanel ppnlSenderAddress = new PropertyPanel();
      ppnlSenderAddress.Text = CommonTerms.SenderAddress;
      pane.Controls.Add( ppnlSenderAddress );

      TextBox txtSenderAddress = new TextBox();
      txtSenderAddress.ID = "TxtSenderAddress" + strLanguageId;
      txtSenderAddress.CssClass = "guiInputText guiInputStandardSize";
      ppnlSenderAddress.Controls.Add( txtSenderAddress );

      CustomValidator cusValSenderAddress = new CustomValidator();
      cusValSenderAddress.ID = "CusValSenderAddress" + strLanguageId;
      cusValSenderAddress.ControlToValidate = txtSenderAddress.ID;
      cusValSenderAddress.ServerValidate += Email_ServerValidate;
      cusValSenderAddress.ErrorMessage = "*";
      ppnlSenderAddress.Controls.Add( cusValSenderAddress );

      PropertyPanel ppnlToAddresses = new PropertyPanel();
      ppnlToAddresses.Text = CommonTerms.ToAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      pane.Controls.Add( ppnlToAddresses );

      TextBox txtToAddresses = new TextBox();
      txtToAddresses.ID = "TxtToAddresses" + strLanguageId;
      txtToAddresses.CssClass = "guiInputText guiInputStandardSize";
      ppnlToAddresses.Controls.Add( txtToAddresses );

      CustomValidator cusValToAddresses = new CustomValidator();
      cusValToAddresses.ID = "CusValToAddresses" + strLanguageId;
      cusValToAddresses.ControlToValidate = txtToAddresses.ID;
      cusValToAddresses.ServerValidate += Email_ServerValidate;
      cusValToAddresses.ErrorMessage = "*";
      ppnlToAddresses.Controls.Add( cusValToAddresses );

      PropertyPanel ppnlCCAddresses = new PropertyPanel();
      ppnlCCAddresses.Text = CommonTerms.CCAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      pane.Controls.Add( ppnlCCAddresses );

      TextBox txtCCAddresses = new TextBox();
      txtCCAddresses.ID = "TxtCCAddresses" + strLanguageId;
      txtCCAddresses.CssClass = "guiInputText guiInputStandardSize";
      ppnlCCAddresses.Controls.Add( txtCCAddresses );

      CustomValidator cusValCCAddresses = new CustomValidator();
      cusValCCAddresses.ID = "CusValCCAddresses" + strLanguageId;
      cusValCCAddresses.ControlToValidate = txtCCAddresses.ID;
      cusValCCAddresses.ServerValidate += Email_ServerValidate;
      cusValCCAddresses.ErrorMessage = "*";
      ppnlCCAddresses.Controls.Add( cusValCCAddresses );

      PropertyPanel ppnlBCCAddresses = new PropertyPanel();
      ppnlBCCAddresses.Text = CommonTerms.BCCAddresses + "<br /><small>" + CommonTerms.SeparateBySemicolon + "</small>";
      pane.Controls.Add( ppnlBCCAddresses );

      TextBox txtBCCAddresses = new TextBox();
      txtBCCAddresses.ID = "TxtBCCAddresses" + strLanguageId;
      txtBCCAddresses.CssClass = "guiInputText guiInputStandardSize";
      ppnlBCCAddresses.Controls.Add( txtBCCAddresses );

      CustomValidator cusValBCCAddresses = new CustomValidator();
      cusValBCCAddresses.ID = "CusValBCCAddresses" + strLanguageId;
      cusValBCCAddresses.ControlToValidate = txtBCCAddresses.ID;
      cusValBCCAddresses.ServerValidate += Email_ServerValidate;
      cusValBCCAddresses.ErrorMessage = "*";
      ppnlBCCAddresses.Controls.Add( cusValBCCAddresses );

      PropertyPanel ppnlTemplate = new PropertyPanel();
      ppnlTemplate.Text = CommonTerms.TemplateFile;
      pane.Controls.Add( ppnlTemplate );

      TemplateFileSelector templateFileSelectorControl = new TemplateFileSelector();
      templateFileSelectorControl.ID = "TemplateFileSelectorControl" + strLanguageId;
      templateFileSelectorControl.InsertEmptyItem = true;
      ppnlTemplate.Controls.Add( templateFileSelectorControl );

      return pane;
    }

    protected override void OnLoad( EventArgs e ) {
      if ( !IsPostBack ) {

        TemplateFileSelectorControl.LoadTemplateFiles();

        TxtName.Text = emailTemplate.Name;
        TxtAlias.Text = emailTemplate.Alias;
        ChkShouldClientReceiveEmail.Checked = emailTemplate.SendEmailToCustomer;

        EmailTemplateSettings defaultSettings = emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == null );
        if ( defaultSettings != null ) {
          TxtSubject.Text = defaultSettings.Subject;
          TxtSenderName.Text = defaultSettings.SenderName;
          TxtSenderAddress.Text = defaultSettings.SenderAddress;
          TxtToAddresses.Text = string.Join( ";", defaultSettings.ToAddresses );
          TxtCCAddresses.Text = string.Join( ";", defaultSettings.CcAddresses );
          TxtBCCAddresses.Text = string.Join( ";", defaultSettings.BccAddresses );

          TemplateFileSelectorControl.Items.TrySelectByValue( defaultSettings.TemplateFile );
        }

        foreach ( Language language in umbracoLanguages ) {

          TemplateFileSelector templateFileSelectorControl = CurrentTabView.FindControl<TemplateFileSelector>( "TemplateFileSelectorControl" + language.id );
          templateFileSelectorControl.LoadTemplateFiles();

          EmailTemplateSettings emailTemplateSettings = emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == language.id );
          if ( emailTemplateSettings != null ) {
            TextBox txtSubject = CurrentTabView.FindControl<TextBox>( "TxtSubject" + language.id );
            TextBox txtSenderName = CurrentTabView.FindControl<TextBox>( "TxtSenderName" + language.id );
            TextBox txtSenderAddress = CurrentTabView.FindControl<TextBox>( "TxtSenderAddress" + language.id );
            TextBox txtToAddresses = CurrentTabView.FindControl<TextBox>( "TxtToAddresses" + language.id );
            TextBox txtCCAddresses = CurrentTabView.FindControl<TextBox>( "TxtCCAddresses" + language.id );
            TextBox txtBCCAddresses = CurrentTabView.FindControl<TextBox>( "TxtBCCAddresses" + language.id );

            txtSubject.Text = emailTemplateSettings.Subject;
            txtSenderName.Text = emailTemplateSettings.SenderName;
            txtSenderAddress.Text = emailTemplateSettings.SenderAddress;
            txtToAddresses.Text = string.Join( ";", emailTemplateSettings.ToAddresses );
            txtCCAddresses.Text = string.Join( ";", emailTemplateSettings.CcAddresses );
            txtBCCAddresses.Text = string.Join( ";", emailTemplateSettings.BccAddresses );

            foreach ( ListItem item in templateFileSelectorControl.Items ) {
              item.Selected = item.Value == emailTemplateSettings.TemplateFile;
              if ( item.Selected ) {
                break;
              }
            }

          }
        }
      }

      base.OnLoad( e );
    }

    private void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        emailTemplate.Name = TxtName.Text;
        emailTemplate.Alias = TxtAlias.Text;
        emailTemplate.SendEmailToCustomer = ChkShouldClientReceiveEmail.Checked;

        string subject = TxtSubject.Text;
        string senderName = TxtSenderName.Text;
        string senderAddress = TxtSenderAddress.Text;
        string toAddresses = TxtToAddresses.Text;
        string ccAddresses = TxtCCAddresses.Text;
        string bccAddresses = TxtBCCAddresses.Text;
        string templateFile = TemplateFileSelectorControl.SelectedValue;

        AddOrUpdateEmailTemplateSettings( emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == null ), subject, senderName, senderAddress, toAddresses, ccAddresses, bccAddresses, templateFile );

        foreach ( Language language in umbracoLanguages ) {
          subject = CurrentTabView.FindControl<TextBox>( "TxtSubject" + language.id ).Text;
          senderName = CurrentTabView.FindControl<TextBox>( "TxtSenderName" + language.id ).Text;
          senderAddress = CurrentTabView.FindControl<TextBox>( "TxtSenderAddress" + language.id ).Text;
          toAddresses = CurrentTabView.FindControl<TextBox>( "TxtToAddresses" + language.id ).Text;
          ccAddresses = CurrentTabView.FindControl<TextBox>( "TxtCCAddresses" + language.id ).Text;
          bccAddresses = CurrentTabView.FindControl<TextBox>( "TxtBCCAddresses" + language.id ).Text;
          templateFile = CurrentTabView.FindControl<TemplateFileSelector>( "TemplateFileSelectorControl" + language.id ).SelectedValue;

          AddOrUpdateEmailTemplateSettings( emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == language.id ), subject, senderName, senderAddress, toAddresses, ccAddresses, bccAddresses, templateFile, language.id );
        }

        emailTemplate.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.EmailTemplateSaved, string.Empty );
      }
    }

    private void AddOrUpdateEmailTemplateSettings( EmailTemplateSettings emailTemplateSettings, string subject, string senderName, string senderAddress, string toAddresses, string ccAddresses, string bccAddresses, string templateFile, long? languageId = null ) {
      if ( !string.IsNullOrEmpty( subject ) || !string.IsNullOrEmpty( senderName ) || !string.IsNullOrEmpty( senderAddress ) || !string.IsNullOrEmpty( toAddresses ) || !string.IsNullOrEmpty( ccAddresses ) || !string.IsNullOrEmpty( bccAddresses ) || !string.IsNullOrEmpty( templateFile ) ) {
        if ( emailTemplateSettings == null ) {
          emailTemplateSettings = new EmailTemplateSettings();
          emailTemplateSettings.LanguageId = languageId;
          emailTemplate.Settings.Add( emailTemplateSettings );
        }

        emailTemplateSettings.Subject = subject;
        emailTemplateSettings.SenderName = senderName;
        emailTemplateSettings.SenderAddress = senderAddress;
        emailTemplateSettings.ToAddresses = new List<string>( toAddresses.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries ) );
        emailTemplateSettings.CcAddresses = new List<string>( ccAddresses.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries ) );
        emailTemplateSettings.BccAddresses = new List<string>( bccAddresses.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries ) );
        emailTemplateSettings.TemplateFile = templateFile;
      } else {
        //Remove it - if it's empty
        if ( emailTemplateSettings != null ) {
          emailTemplate.Settings.Remove( emailTemplateSettings );
        }
      }
    }

    protected void Email_ServerValidate( object source, ServerValidateEventArgs args ) {
      args.IsValid = true;

      string[] tokens = args.Value.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
      foreach ( string token in tokens ) {
        if ( !string.IsNullOrEmpty( token ) ) {
          if ( !Regex.IsMatch( token, @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" ) ) {
            args.IsValid = false;
            break;
          }
        }
      }
    }

  }
}