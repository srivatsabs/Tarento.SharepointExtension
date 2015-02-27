using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// A series of extension methods the extend the base SharePoint objects. Primarily used to fill the gap in 
    /// SharePoint object checking by utilizing LINQ for objects and discerning if objects exist before using them.
    /// Previously many developers used try/catch (against best practices) to find out if objects existed.
    /// </summary>
    /// <remarks></remarks>
    public static class SharePointExtensions
    {
        #region List Discovery
        /// <summary>
        /// Checks to see if the named list exists in the web
        /// </summary>
        /// <param name="web">The web that the list is in</param>
        /// <param name="ListName">Name of the list.</param>
        /// <returns>true if the list exists in the web</returns>
        /// <remarks></remarks>
        public static bool ListExists(this SPWeb web, string ListName)
        {
            return web.Lists.Cast<SPList>().Any(list => string.Compare(list.Title, ListName, true) == 0);
        }

        /// <summary>
        /// Checks to see if the list exists in the web
        /// </summary>
        /// <param name="web">The web that the list is in</param>
        /// <param name="ListID">The list ID</param>
        /// <returns>true if the list exists in the web</returns>
        /// <remarks></remarks>
        public static bool ListExists(this SPWeb web, Guid ListID)
        {
            return web.Lists.Cast<SPList>().Any(list => string.Compare(list.ID.ToString(), ListID.ToString(), true) == 0);
        }

        /// <summary>
        /// Checks to see if the named list exists in the web
        /// </summary>
        /// <param name="web">The web that the list is in</param>
        /// <param name="ListID">The list ID</param>
        /// <param name="list">out param, fills with the discovered list, null if no list exists</param>
        /// <returns>true if the list exists in the web</returns>
        /// <example>Using the ListExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// if(web.ListExists("6CC97642-0935-441C-941D-4FB21321B475", out list)
        /// {
        ///     //do something with the list
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper(...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ListExists(this SPWeb web, Guid ListID, out SPList list)
        {
            list = web.Lists.Cast<SPList>().FirstOrDefault(l => Guid.Equals(l.ID, ListID));
            return !list.IsNothing();
        }

        /// <summary>
        /// Checks to see if the named list exists in the web
        /// </summary>
        /// <param name="web">The web that the list is in</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="list">out param, fills with the discovered list, null if no list exists</param>
        /// <returns>true if the list exists in the web</returns>
        /// <example>Using the ListExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// if(web.ListExists("List Name", out list)
        /// {
        ///     //do something with the list
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper(...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ListExists(this SPWeb web, string listName, out SPList list)
        {
            list = web.Lists.Cast<SPList>().FirstOrDefault(l => string.Equals(l.Title, listName, StringComparison.InvariantCultureIgnoreCase));
            return !list.IsNothing();
        }              

        /// <summary>
        /// Checks to see if the named document library exists in the web
        /// </summary>
        /// <param name="web">The web that the document library is in</param>
        /// <param name="libraryName">Name of the document library</param>
        /// <param name="documentLibrary">out param, fills with the discovered document library, null if no library exists</param>
        /// <returns>true if the list exists in the web</returns>
        /// <example>Using the LibraryExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPDocumentLibrary library = null;
        /// if(web.LibraryExists("library name", out library)
        /// {
        ///     //do something with the library
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper(...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool LibraryExists(this SPWeb web, string libraryName, out SPDocumentLibrary documentLibrary)
        {
            documentLibrary = null;
            try
            {
                SPList list = web.Lists.Cast<SPList>().FirstOrDefault(l => string.Equals(l.Title, libraryName, StringComparison.InvariantCultureIgnoreCase));
                if (!list.IsNothing())
                {
                    documentLibrary = list as SPDocumentLibrary;
                    return true;
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region Field Discovery
        /// <summary>
        /// Checks to see if the named field exists in the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="FieldName">Internal name of the field</param>
        /// <param name="field">out param, fills with the discovered field, null if no field exists in the list</param>
        /// <returns>true if the field exists in the list</returns>
        /// <example>Using the FieldExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPField field = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(list.FieldExists("field name", out field)
        ///     {
        ///         //do something with the field
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No Field",...);
        ///     }
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool FieldExists(this SPList list, string FieldName, out SPField field)
        {
            field = list.Fields.Cast<SPField>().FirstOrDefault(f => string.Equals(f.InternalName, FieldName, StringComparison.InvariantCultureIgnoreCase));
            if (field.IsNothing())
            {
                field = list.Fields.Cast<SPField>().FirstOrDefault(f => string.Equals(f.Title, FieldName, StringComparison.InvariantCultureIgnoreCase));
            }
            return !field.IsNothing();
        }

        /// <summary>
        /// Checks to see if the named field exists in the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <returns>true if the field exists in the list</returns>
        /// <remarks></remarks>
        public static bool FieldExists(this SPList list, string InternalName)
        {
            if (list != null)
            {
                return list.Fields.ContainsFieldWithStaticName(InternalName);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the named field exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <param name="field">out param, fills with the discovered field, null if no field exists in the web</param>
        /// <returns>true if the field exists in the web</returns>
        /// <example>Using the FieldExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPField field = null;
        /// if(web.FieldExists("field name", out field)
        /// {
        ///     //do something with the field
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No Field",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool FieldExists(this SPWeb web, string InternalName, out SPField field)
        {
            field = web.Fields.Cast<SPField>().FirstOrDefault(f => string.Equals(f.InternalName, InternalName, StringComparison.InvariantCultureIgnoreCase));
            return !field.IsNothing();
        }

        /// <summary>
        /// Checks to see if the named field exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <returns>true if the field exists in the web</returns>
        /// <remarks></remarks>
        public static bool FieldExists(this SPWeb web, string InternalName)
        {
            if (web != null)
            {
                return web.Fields.ContainsFieldWithStaticName(InternalName);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the named field exists in the content type
        /// </summary>
        /// <param name="ctype">The content type object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <returns>true if the field exists in the content type</returns>
        /// <remarks></remarks>
        public static bool FieldExists(this SPContentType ctype, string InternalName)
        {
            return ctype.Fields.ContainsFieldWithStaticName(InternalName);
        }

        /// <summary>
        /// Checks to see if the named field exists in the content type
        /// </summary>
        /// <param name="ctype">The content type object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <param name="field">out param, fills with the discovered field, null if no field exists in the content type</param>
        /// <returns>true if the field exists in the content type</returns>
        /// <example>Using the FieldExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPContentType ctype = null;
        /// SPField field = null;
        /// if(web.ContentTypeExists("content type name", out ctype)
        /// {
        ///     if(ctype.FieldExists("field name", out field)
        ///     {
        ///         //do something with the field
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No Field",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No ContentType",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool FieldExists(this SPContentType ctype, string InternalName, out SPField field)
        {
            field = ctype.Fields.Cast<SPField>().FirstOrDefault(f => string.Equals(f.InternalName, InternalName, StringComparison.InvariantCultureIgnoreCase));
            return !field.IsNothing();
        }

        /// <summary>
        /// Checks to see if the named field exists in the list item
        /// </summary>
        /// <param name="item">The list item object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <returns>true if the field exists in the list item</returns>
        /// <remarks></remarks>
        public static bool FieldExists(this SPListItem item, string InternalName)
        {//1.6
            return item.Fields.ContainsFieldWithStaticName(InternalName);
        }

        /// <summary>
        /// Checks to see if the named field exists in the list item
        /// </summary>
        /// <param name="item">The list item object</param>
        /// <param name="InternalName">Internal name of the field</param>
        /// <param name="field">out param, fills with the discovered field, null if no field exists in the list item</param>
        /// <returns>true if the field exists in the list item</returns>
        /// <example>Using the FieldExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem item = null;
        /// SPField field = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("title", out item)
        ///     {
        ///         if(item.FieldExists("title", out item)
        ///         {
        ///             //do something with the field
        ///         }
        ///         else
        ///         {
        ///             Logger.TraceToDeveloper("No Field",...);
        ///         }
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool FieldExists(this SPListItem item, string InternalName, out SPField field)
        {//1.6
            field = item.Fields.Cast<SPField>().FirstOrDefault(f => string.Equals(f.InternalName, InternalName, StringComparison.InvariantCultureIgnoreCase));
            return !field.IsNothing();
        }

        #endregion

        #region Folder Discovery
        /// <summary>
        /// Checks to see if the named folder exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="FolderUrl">The folder URL</param>
        /// <returns>true if the folder exists in the web</returns>
        /// <remarks></remarks>
        public static bool FolderExists(this SPWeb web, string FolderUrl)
        {//1.5
            SPFolder folder = null;
            return web.FolderExists(FolderUrl, out folder);
        }

        /// <summary>
        /// Checks to see if the named folder exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="FolderUrl">The folder URL.</param>
        /// <param name="folder">out param, fills with the discovered folder, null if no folder exists in the web</param>
        /// <returns>true if the folder exists in the web</returns>
        /// <example>Using the FolderExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPFolder folder = null;
        /// if(web.FolderExists("folder/url", out folder)
        ///     //do something with the folder
        /// else
        ///     //No folder logic
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool FolderExists(this SPWeb web, string FolderUrl, out SPFolder folder)
        {//1.5
            folder = web.Folders.Cast<SPFolder>().FirstOrDefault(l => string.Equals(l.Url, FolderUrl, StringComparison.InvariantCultureIgnoreCase));
            return !folder.IsNothing();
        }

        /// <summary>
        /// Checks to see if the named subfolder exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="FolderUrl">The folder URL.</param>
        /// <returns>true if the subfolder exists in the web</returns>
        /// <remarks></remarks>
        public static bool SubFolderExists(this SPWeb web, string FolderUrl)
        {//1.5
            SPFolder folder = null;
            return web.SubFolderExists(FolderUrl, out folder);
        }

        /// <summary>
        /// Checks to see if the named subfolder exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="FolderUrl">The folder URL.</param>
        /// <param name="folder">out param, fills with the discovered folder, null if no folder exists in the web</param>
        /// <returns>true if the subfolder exists in the web</returns>
        /// <example>Using the SubFolderExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPFolder folder = null;
        /// if(web.SubFolderExists("subfolder/url", out folder)
        ///     //do something with the folder
        /// else
        ///     //No folder logic
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool SubFolderExists(this SPWeb web, string FolderUrl, out SPFolder folder)
        {//1.5
            folder = web.RootFolder.SubFolders.Cast<SPFolder>().FirstOrDefault(l => string.Equals(l.Url, FolderUrl, StringComparison.InvariantCultureIgnoreCase));
            return !folder.IsNothing();
        }
        #endregion

        #region Item Discovery and Eval
        /// <summary>
        /// Checks to see if an item with the provided title exists in the list(one or many if the titles are not unique)
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="title">The title value to find</param>
        /// <returns>true if the item with the specified title exists in the list</returns>
        /// <remarks></remarks>
        public static bool ItemExists(this SPList list, string title)
        {
            SPListItem[] item = null;
            return list.ItemExists(title, out item);
        }

        /// <summary>
        /// Checks to see if an item with the provided title exists in the list(one or many if the titles are not unique). 
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="title">The title value to find</param>
        /// <param name="item">out param, fills with the discovered array of list items, null if no item exists in the list</param>
        /// <returns>true if the item with the specified title exists in the list</returns>
        /// <example>Using the ItemExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem[] items;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("title", out items)
        ///     {
        ///         //do something with items
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Do not use this method on large lists. It is intended to be used on list with 
        /// fewer than 5000 items. If you do intend to use this method on a large list, ensure the 
        /// title column is unique and indexed.
        /// </remarks>
        public static bool ItemExists(this SPList list, string title, out SPListItem[] item)
        {
            item = (from i in list.Items.OfType<SPListItem>() where i["Title"].ToString().Equals(title, StringComparison.InvariantCultureIgnoreCase) select i).ToArray<SPListItem>();
            return !item.IsNothing() && item.Count<SPListItem>() > 0;
        }

        /// <summary>
        /// Checks to see if an item with the provided title exists in the list and returns the first occurrance
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="title">The title value to find</param>
        /// <param name="item">out param, returns the first occurrance or the found item, null if no matching item exists in the list</param>
        /// <returns>true if the item with the specified title exists in the list</returns>
        /// <example>Using the ItemExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem item = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("title", out item)
        ///     {
        ///         //do something with item
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Do not use this method on large lists. It is intended to be used on list with 
        /// fewer than 5000 items. If you do intend to use this method on a large list, ensure the 
        /// title column is unique and indexed.
        /// </remarks>
        public static bool ItemExists(this SPList list, string title, out SPListItem item)
        {
            item = (from i in list.Items.OfType<SPListItem>() where i["Title"].ToString().Equals(title, StringComparison.InvariantCultureIgnoreCase) select i).FirstOrDefault<SPListItem>();
            return !item.IsNothing();
        }

        /// <summary>
        /// Checks to see if an item with the provided field name and field value exists in the list
        /// and returns the first occurrance.
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="field_name">the internal name of the field to use for comparison of the value</param>
        /// <param name="field_value">The value to find</param>
        /// <param name="item">out param, returns the first occurrance or the found item, null if no matching item exists in the list</param>
        /// <returns>true if the item with the specified title exists in the list</returns>
        /// <example>Using the ItemExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem item = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("field name", "field value", out item)
        ///     {
        ///         //do something with item
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Do not use this method on large lists. It is intended to be used on list with 
        /// fewer than 5000 items. If you do intend to use this method on a large list, ensure the 
        /// title column is unique and indexed.
        /// </remarks>
        public static bool ItemExists(this SPList list, string field_name, string field_value, out SPListItem item)
        {
            item = null;
            if (list.FieldExists(field_name))
            {
                item = (from i in list.Items.OfType<SPListItem>() where i[field_name].ToString().Equals(field_value, StringComparison.InvariantCultureIgnoreCase) select i).FirstOrDefault<SPListItem>();
                return !item.IsNothing();
            }
            return false;
        }

        /// <summary>
        /// Locates a folder in a document library and returns the folder (if it exists) in the out param.
        /// </summary>
        /// <param name="library">The document library</param>
        /// <param name="folderNameOrUrl">The folder name or URL.</param>
        /// <param name="folder">The folder object out param</param>
        /// <returns>true if the folder exists in the library</returns>
        /// <remarks></remarks>
        public static bool FolderExists(this SPDocumentLibrary library, string folderNameOrUrl, out SPFolder folder)
        {
            folder = (
                from i in library.Folders.OfType<SPListItem>()
                where (i.Name.Equals(folderNameOrUrl, StringComparison.InvariantCultureIgnoreCase) ||
                        i.Url.Equals(folderNameOrUrl, StringComparison.InvariantCultureIgnoreCase))
                select i.Folder).FirstOrDefault<SPFolder>();
            return folder != null;
        }

        /// <summary>
        /// Gets the string representation of the field value in an item
        /// </summary>
        /// <param name="item">The item object</param>
        /// <param name="field_name">The internal name of the field whose value should be returned.</param>
        /// <returns>the string value of the object</returns>
        /// <example>Using the GetItemFieldValue method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem item = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("field name", "field value", out item)
        ///     {
        ///         string value = item.GetItemFieldValue("field name");
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks>Checks that the field is valid and exists in the item and that it is not null</remarks>
        public static string GetItemFieldValue(this SPListItem item, string field_name)
        {
            if (ItemFieldExistsAndIsNotNull(item, field_name))
            {
                return item[field_name].ToSafeString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the column values for a field in a list
        /// </summary>
        /// <param name="list">The list to use</param>
        /// <param name="field_name">The internal field_name whose column values will be returned</param>
        /// <returns>List of column values</returns>
        /// <example>Using the GetListColumnValues method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     List&lt;string&gt; columns = list.GetListColumnValues("field name");
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static List<string> GetListColumnValues(this SPList list, string field_name)
        {
            return (from i in list.Items.OfType<SPListItem>() select i[field_name].ToString()).ToList<string>();
        }

        /// <summary>
        /// Gets the unique list of column values.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="field_name">The field_name.</param>
        /// <returns>list of unique column values</returns>
        /// <example>Using the GetUniqueListColumnValues method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     List&lt;string&gt; columns = list.GetUniqueListColumnValues("field name");
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static List<string> GetUniqueListColumnValues(this SPList list, string field_name)
        {
            return (from i in list.Items.OfType<SPListItem>() select i[field_name].ToString()).Distinct<string>().ToList<string>();
        }

        /// <summary>
        /// Checks to see if the field exists on the item and that the value in the field is not null
        /// </summary>
        /// <param name="item">The item object</param>
        /// <param name="field_name">The name of the field</param>
        /// <returns>true if the item exists and has a value</returns>
        /// <remarks></remarks>
        public static bool ItemFieldExistsAndIsNotNull(this SPListItem item, string field_name)
        {
            try
            {
                SPField field = null;
                if (item.FieldExists(field_name, out field))
                {
                    if (item[field_name] != null)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Checks to see if the field exists on the item and that the value in the field is not null
        /// </summary>
        /// <param name="item">The item object</param>
        /// <param name="field_name">The name of the field</param>
        /// <param name="value">out parameter that returns the value in the field</param>
        /// <returns>true if the item exists and has a value</returns>
        /// <example>Using the FieldExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPListItem item = null;
        /// string value = string.empty;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ItemExists("title", out item)
        ///     {
        ///         if(item.ItemFieldExistsAndIsNotNull("title", out value)
        ///         {
        ///             //do something with the value
        ///         }
        ///         else
        ///         {
        ///             Logger.TraceToDeveloper("No Field or field value is null",...);
        ///         }
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ListItem",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ItemFieldExistsAndIsNotNull(this SPListItem item, string field_name, out string value)
        {
            value = string.Empty;
            try
            {
                if (item.FieldExists(field_name))
                {
                    value = item[field_name].ToSafeString();
                    if (value != null)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region Content Type Discovery
        /// <summary>
        /// Checks to see if the content type exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="cid">The content type id (as SPContentTypeId object).</param>
        /// <returns>true if the content type exists in the web</returns>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPWeb web, SPContentTypeId cid)
        {//1.6
            return web.ContentTypes.Cast<SPContentType>().Any(ctype => ctype.Id.CompareTo(cid) == 0);
        }

        /// <summary>
        /// Checks to see if the content type exists in the web and passes back the discovered object
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="ContentTypeName">Name of the content type</param>
        /// <param name="ContentType">out param, returns the content type if one is located in the web</param>
        /// <returns>true if the content type exists in the web</returns>
        /// <example>Using the ContentTypeExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPContentType ctype = null;
        /// SPField field = null;
        /// if(web.ContentTypeExists("content type name", out ctype)
        /// {
        ///     //do something with the content type
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No ContentType",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPWeb web, string ContentTypeName, out SPContentType ContentType)
        {
            ContentType = null;
            if (web.ContentTypes.Cast<SPContentType>().Any(ctype => string.Compare(ctype.Name, ContentTypeName, true) == 0))
            {
                ContentType = web.ContentTypes[ContentTypeName];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the content type exists in the web
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="ContentTypeName">Name of the content type</param>
        /// <returns>true if the content type exists in the web</returns>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPWeb web, string ContentTypeName)
        {
            return web.ContentTypes.Cast<SPContentType>().Any(ctype => string.Compare(ctype.Name, ContentTypeName, true) == 0);
        }

        /// <summary>
        /// Checks to see if the content type exists in the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="cid">The content type id (as SPContentTypeId object).</param>
        /// <returns>true if the content type exists in the list</returns>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPList list, SPContentTypeId cid)
        {//1.6
            return list.ContentTypes.Cast<SPContentType>().Any(ctype => ctype.Id.CompareTo(cid) == 0);
        }

        /// <summary>
        /// Contents the type exists.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="ContentTypeName">Name of the content type.</param>
        /// <returns>true if the content type exists</returns>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPList list, string ContentTypeName)
        {
            return list.ContentTypes.Cast<SPContentType>().Any(ctype => string.Compare(ctype.Name, ContentTypeName, true) == 0);
        }

        /// <summary>
        /// Checks to see if the content type exists in the list and passes back the discovered object
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="ContentTypeName">Name of the content type.</param>
        /// <param name="ContentType">out param, returns the content type if one is located in the web</param>
        /// <returns>true if the content type exists in the list</returns>
        /// <example>Using the ContentTypeExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPContentType ctype = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(list.ContentTypeExists("content type name", out ctype)
        ///     {
        ///         //do something with the content type
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No ContentType",...);
        ///     }
        /// }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ContentTypeExists(this SPList list, string ContentTypeName, out SPContentType ContentType)
        {
            ContentType = null;
            if (list.ContentTypes.Cast<SPContentType>().Any(ctype => string.Compare(ctype.Name, ContentTypeName, true) == 0))
            {
                ContentType = list.ContentTypes[ContentTypeName];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a site column to a content type
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="contentTypeName">Name of the content type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>true if the field was successfully linked to the content type</returns>
        /// <remarks></remarks>
        public static bool AddSiteColumnToContentType(this SPWeb web, string contentTypeName, string fieldName)
        {
            return AddSiteColumnToContentType(web, contentTypeName, fieldName, false);
        }
        
        /// <summary>
        /// Adds a site column to a content type
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="contentTypeName">Name of the content type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="hidden">Whether or not the linked field should be hidden</param>
        /// <returns>true if the field was successfully linked to the content type</returns>
        /// <remarks></remarks>
        public static bool AddSiteColumnToContentType(this SPWeb web, string contentTypeName, string fieldName, bool hidden)
        {
            SPContentType ctype = null;
            if(web.ContentTypeExists(contentTypeName, out ctype))
            {
                SPField field = null;
                if (web.FieldExists(fieldName, out field))
                {
                    if (!ctype.FieldExists(fieldName))
                    {
                        SPFieldLink fieldLink = new SPFieldLink(field);
                        fieldLink.Hidden = hidden;
                        ctype.FieldLinks.Add(fieldLink);
                        ctype.Update(true);
                        web.Update();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Edits a Site Column to Content Type Mapping
        /// </summary>
        /// <param name="web">The web object</param>
        /// <param name="contentTypeName">Name of the content type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="hidden">Whether or not the linked field should be hidden</param>
        /// <returns>true if the field was successfully updated to the content type</returns>
        /// <remarks></remarks>
        public static bool EditSiteColumnToContentType(this SPWeb web, string contentTypeName, string fieldName, bool hidden)
        {
            SPContentType ctype = null;
            if (web.ContentTypeExists(contentTypeName, out ctype))
            {
                if (ctype.Fields.ContainsFieldWithStaticName(fieldName))
                {
                    SPField field = ctype.Fields.GetFieldByInternalName(fieldName);
                    SPFieldLink fieldLink = ctype.FieldLinks[field.Id];
                    fieldLink.Hidden = hidden;
                    ctype.Update(true);
                    web.Update();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove site columns from News content type
        /// </summary>
        /// <param name="web">The current web</param>
        /// <param name="contentTypeName">content type name</param>
        /// <param name="fieldName">internal field name of site column</param>
        /// <returns>returns true if the field was removed from the content type</returns>
        /// <remarks></remarks>
        public static bool RemoveSiteColumnFromContentType(this SPWeb web, string contentTypeName, string fieldName)
        {
            SPContentType ctype = null;
            if (web.ContentTypeExists(contentTypeName, out ctype))
            {
                SPField field = null;
                if (web.FieldExists(fieldName, out field))
                {
                    if (!ctype.FieldExists(fieldName))
                    {
                        ctype.FieldLinks.Delete(fieldName);
                        ctype.Update(true);
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region View Discovery
        /// <summary>
        /// Checks to see if the view exists in the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="ViewName">The name of the view</param>
        /// <returns>true if the view exists in the list</returns>
        /// <remarks></remarks>
        public static bool ViewExists(this SPList list, string ViewName)
        {//1.6
            SPView view = list.Views.Cast<SPView>().FirstOrDefault(v => string.Equals(v.Title, ViewName, StringComparison.InvariantCultureIgnoreCase));
            return !view.IsNothing();
        }

        /// <summary>
        /// Checks to see if the view exists in the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="ViewName">The name of the view</param>
        /// <param name="view">out param, returns the discovered view if it exists in the list.</param>
        /// <returns>true if the view exists in the list</returns>
        /// <example>Using the ViewExists method
        /// <code>
        /// SPWeb web = site.RootWeb;
        /// SPList list = null;
        /// SPView view = null;
        /// if(web.ListExists("list name", out list)
        /// {
        ///     if(List.ViewExists("title", out item)
        ///     {
        ///         //do something with view
        ///     }
        ///     else
        ///     {
        ///         Logger.TraceToDeveloper("No View",...);
        ///     }
        /// else
        /// {
        ///     Logger.TraceToDeveloper("No List",...);
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static bool ViewExists(this SPList list, string ViewName, out SPView view)
        {//1.6
            view = list.Views.Cast<SPView>().FirstOrDefault(v => string.Equals(v.Title, ViewName, StringComparison.InvariantCultureIgnoreCase));
            return !view.IsNothing();
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Renders the field control into the HTML equivalent
        /// </summary>
        /// <param name="control">The control to be converted</param>
        /// <returns>HTML markup of the control</returns>
        /// <example>Using the RenderFieldControl method
        /// <code>
        /// SPList list = web.Lists["list name"];
        /// SPFieldUser userField = null;
        /// if(list.FieldExists("users", out userField)
        /// {
        ///     string userFieldHTML = userField.RenderFieldControl();
        /// }
        /// else
        /// {
        ///     //no field exists logic
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static string RenderFieldControl(this BaseFieldControl control)
        {
            try
            {
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    control.RenderControl(hw);
                    hw.Flush();
                    return sw.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Removes the content type entry from create menu on the list
        /// </summary>
        /// <param name="list">The list object</param>
        /// <param name="content_type_name">The content type name</param>
        /// <example>Using the RemoveContentTypeFromCreate method
        /// <code>
        /// SPList list = web.Lists["list name"];
        /// list.RemoveContentTypeFromCreate("Item");
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public static void RemoveContentTypeFromCreate(this SPList list, string content_type_name)
        {
            SPFolder root = list.RootFolder;
            IList<SPContentType> ct_list = root.ContentTypeOrder;
            foreach (SPContentType ct in ct_list)
            {
                if (ct.Name.Equals(content_type_name, StringComparison.InvariantCultureIgnoreCase))
                {
                    ct_list.Remove(ct);
                    break;
                }
            }
            root.UniqueContentTypeOrder = ct_list;
            root.Update();
        }

        /// <summary>
        /// Gets a new list item and presets the specified content type.
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="cid">The id of the content type.</param>
        /// <returns>A list item object with a preset contenttype field value</returns>
        /// <remarks></remarks>
        public static SPListItem GetTypedItem(this SPList list, SPContentTypeId cid)
        {
            SPContentType ctype = list.ContentTypes[cid];
            SPListItem item = list.Items.Add();
            item["ContentType"] = ctype.Id;
            item.SystemUpdate(false);
            return item;
        }

        /// <summary>
        /// Sets page as welcome page (home)
        /// </summary>
        /// <param name="web">The web for which welcome page needs to be set</param>
        /// <param name="welcomePageUrl">partial url of welcome page. For example, /sitepages/welcome.aspx</param>
        public static void SetWelcomePage(this SPWeb web, string welcomePageUrl)
        {
            SPFolder folder = web.RootFolder;
            folder.WelcomePage = welcomePageUrl;
            folder.Update();
        }
        #endregion

        #region User Profile Helpers
        /// <summary>
        /// Gets my site host base URL.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns>the url for the mysite host</returns>
        /// <remarks></remarks>
        public static string GetMySiteHostUrl(this SPSite site)
        {
            string url = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPServiceContext context = SPServiceContext.GetContext(site);
                if (UserProfileManager.IsAvailable(context))
                {
                    url = new UserProfileManager(context).MySiteHostUrl;
                }
                else
                {
                    throw new ApplicationException("User Profile Manager did not exist or was not available in the current context."); 
                }
            });
            return url;
        }

        /// <summary>
        /// Gets the profile values.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="account">The account.</param>
        /// <returns>a distionary that contains the profile values for each key passed</returns>
        /// <remarks></remarks>
        public static Dictionary<string, object> GetProfileValues(this SPSite site, string[] properties, string account)
        {
            Dictionary<string, object> values = new Dictionary<string, object>(properties.Length - 1);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPServiceContext context = SPServiceContext.GetContext(site);
                if (UserProfileManager.IsAvailable(context))
                {
                    UserProfileManager userProfileManager = new UserProfileManager(context);

                    if (account.ToLower().Contains("system account") || account.Equals("sharepoint\\system", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ArgumentOutOfRangeException("Unable to retreive profile value for the system account");
                    }
                    UserProfile profile = userProfileManager.GetUserProfile(account);
                    if (profile != null)
                    {
                        foreach (string key in properties)
                        {
                            if (profile[key] == null || profile[key].Value == null)
                            {
                                values.Add(key, null);
                            }
                            else
                            {
                                values.Add(key, profile[key].Value);
                            }
                        }
                    }
                    else
                    {
                        throw new UserProfileException(string.Format("User profile for account {0} did not exist", account));
                    }
                }
                else
                {
                    throw new ApplicationException("User Profile Manager did not exist or was not available in the current context."); 
                }
            });
            return values;
        }

        /// <summary>
        /// Gets the profile value.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="property">The property.</param>
        /// <param name="account">The account.</param>
        /// <returns>The value from the profile</returns>
        /// <remarks></remarks>
        public static object GetProfileValue(this SPSite site, string property, string account)
        {
            object value = null;
            if (account.ToLower().Contains("system account") || account.Equals("sharepoint\\system", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("Unable to retreive profile value for the system account");
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPServiceContext context = SPServiceContext.GetContext(site);
                if (UserProfileManager.IsAvailable(context))
                {
                    UserProfile profile = new UserProfileManager(context).GetUserProfile(account);
                    if (profile != null)
                    {
                        if (profile[property] != null || profile[property].Value != null)
                        {
                            value = profile[property].Value;
                        }
                    }
                    else
                    {
                        throw new UserProfileException(string.Format("User profile for account {0} did not exist", account));
                    }
                }
                else
                {
                    throw new ApplicationException("User Profile Manager did not exist or was not available in the current context."); 
                }
            });
            return value;
        }
        #endregion

        #region File and Folder Retreival
        /// <summary>
        /// Gets the file from within the provided folder object
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An SPFile object is the file exists in the folder, otherwise returns null</returns>
        /// <remarks></remarks>
        public static SPFile GetFile(this SPFolder folder, string fileName)
        {
            return (from f in folder.Files.OfType<SPFile>() where f.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase) select f).FirstOrDefault<SPFile>();
        }

        /// <summary>
        /// Gets the file stream from a file in a document library
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>a streaming object</returns>
        /// <remarks>This will return the first occurance of the file of this name in the document library</remarks>
        public static Stream GetFileStream(this SPDocumentLibrary library, string fileName)
        {
            SPListItemCollection folders = library.Folders;
            SPFile file = (from f in folders.OfType<SPFolder>() where f.GetFile(fileName) != null select f.GetFile(fileName)).FirstOrDefault<SPFile>();
            if (file.Exists)
            {
                return file.OpenBinaryStream(SPOpenBinaryOptions.SkipVirusScan);
            }
            return null;
        }
        
        /// <summary>
        /// Gets the file from the site folder structure (url based). If the file does not exist, there is an option to create the file and add content.
        /// </summary>
        /// <param name="folder">The folder the file is located in</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="CreateIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <param name="Contents">The contents that should be added to the newly created file.</param>
        /// <returns>A SPFile object that existed or was created</returns>
        /// <remarks></remarks>
        public static SPFile GetFile(this SPFolder folder, string fileName, bool CreateIfNotExists, Byte[] Contents)
        {
            SPFile file = (from f in folder.Files.OfType<SPFile>() where f.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase) select f).FirstOrDefault<SPFile>();
            if (file == null && CreateIfNotExists)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate() { file = folder.Files.Add(fileName, Contents); });
            }
            return file;
        }

        /// <summary>
        /// Gets the folder from the passed web object
        /// </summary>
        /// <param name="web">The web that contains the folder</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="CreateIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns>SPFolder object OR null if the folder does not exist and the CreateIfNotExists value is set to false</returns>
        /// <remarks></remarks>
        public static SPFolder GetFolder(this SPWeb web, string folderName, bool CreateIfNotExists)
        {
            SPFolder folder = null;
            try
            {
                folder = (from f in web.Folders.OfType<SPFolder>() where f.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase) select f).FirstOrDefault<SPFolder>();
                if (CreateIfNotExists && (folder == null || !folder.Exists))
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate() { web.Folders.Add(folderName); });
                }
            }
            catch { }
            return folder;
        }
        
        /// <summary>
        /// Gets the sub folder from a base folder
        /// </summary>
        /// <param name="basefolder">The basefolder which contains the folder</param>
        /// <param name="folderName">Name of the folder that should be retreived</param>
        /// <param name="CreateIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns>SPFolder object OR null if the folder does not exist and the CreateIfNotExists value is set to false</returns>
        /// <remarks></remarks>
        public static SPFolder GetSubFolder(this SPFolder basefolder, string folderName, bool CreateIfNotExists)
        {
            SPFolder folder = null;
            try
            {
                folder = (from f in basefolder.SubFolders.OfType<SPFolder>() where f.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase) select f).FirstOrDefault<SPFolder>();
                if (CreateIfNotExists && (folder == null || !folder.Exists))
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate() { basefolder.SubFolders.Add(folderName); });
                }
            }
            catch { }
            return folder;
        }
        #endregion

        #region SPField Converters
        /// <summary>
        /// Converts a string to a SP Field Url Object
        /// </summary>
        /// <param name="url_field_value">The url field value.</param>
        /// <returns>a SPFieldUrlValue object</returns>
        /// <remarks></remarks>
        public static SPFieldUrlValue ToSPFieldUrl(this string url_field_value)
        {
            if (!string.IsNullOrEmpty(url_field_value))
            {
                return new SPFieldUrlValue(url_field_value);
            }
            return new SPFieldUrlValue();
        }

        /// <summary>
        /// Converts a string to a SP Field Lookup Object
        /// </summary>
        /// <param name="lookup_field_value">The lookup field value.</param>
        /// <returns>a SPFieldLookupValue object</returns>
        /// <remarks></remarks>
        public static SPFieldLookupValue ToSPFieldLookup(this string lookup_field_value)
        {
            if (!string.IsNullOrEmpty(lookup_field_value))
            {
                return new SPFieldLookupValue(lookup_field_value);
            }
            return new SPFieldLookupValue();
        }
        #endregion
    }
}
