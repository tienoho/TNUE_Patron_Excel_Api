using System;
using System.Collections.Generic;
using System.DirectoryServices;
using TNUE_Patron_Excel.DBConnect;

namespace TNUE_Patron_Excel.Ldap
{
	internal class ModelLdap
	{
		public enum Property
		{
			title,
			displayName,
			sn,
			l,
			postalCode,
			physicalDeliveryOfficeName,
			telephoneNumber,
			mail,
			givenName,
			initials,
			co,
			department,
			company,
			streetAddress,
			employeeID,
			mobile,
			userPrincipalName,
			userPassword
		}

		private ConectLdap cnLdap = new ConectLdap();

		private LdapField ldap = new ReadWriterConfig().ReadConfigLdap();

		public string CreateUser(User user)
		{
			try
			{
				cnLdap.Connect();
				using (DirectoryEntry directoryEntry = cnLdap.entry)
				{
					using (DirectoryEntry directoryEntry2 = directoryEntry.Children.Add("uid=" + user.userLogin, user.objectClass))
					{
						if (user.objectClass != null && user.objectClass.Length > 0)
						{
							directoryEntry2.Properties["objectClass"].Add(user.objectClass);
						}
						if (user.cn != null && user.cn.Length > 0)
						{
							directoryEntry2.Properties["cn"].Add(user.cn);
						}
						if (user.sn != null && user.sn.Length > 0)
						{
							directoryEntry2.Properties["sn"].Add(user.sn);
						}
						if (user.userPassword != null && user.userPassword.Length > 0)
						{
							directoryEntry2.Properties["userPassword"].Add(user.userPassword);
						}
						if (user.telephoneNumber != null && user.telephoneNumber.Length > 0)
						{
							directoryEntry2.Properties["telephoneNumber"].Add(user.telephoneNumber);
						}
						if (user.userMail != null && user.userMail.Length > 0)
						{
							directoryEntry2.Properties["mail"].Add(user.userMail);
						}
						directoryEntry2.CommitChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			return "";
		}

		public void SetAdInfo(string objectFilter, Property objectName, string objectValue)
		{
			cnLdap.Connect();
			using (DirectoryEntry searchRoot = cnLdap.entry)
			{
				using (DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot))
				{
					directorySearcher.Filter = "(cn=" + objectFilter + ")";
					directorySearcher.PropertiesToLoad.Add(string.Concat(objectName) ?? "");
					SearchResult searchResult = directorySearcher.FindOne();
					if (searchResult != null)
					{
						using (DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry())
						{
							if (!string.IsNullOrEmpty(objectValue))
							{
								if (searchResult.Properties.Contains(string.Concat(objectName) ?? ""))
								{
									directoryEntry.Properties[string.Concat(objectName) ?? ""].Value = objectValue;
								}
								else
								{
									directoryEntry.Properties[string.Concat(objectName) ?? ""].Add(objectValue);
								}
								directoryEntry.CommitChanges();
							}
						}
					}
				}
			}
		}

		public List<User> GetAllListUser()
		{
			List<User> list = null;
			cnLdap.Connect();
			try
			{
				using (DirectoryEntry directoryEntry = cnLdap.entry)
				{
					using (DirectorySearcher directorySearcher2 = new DirectorySearcher(directoryEntry))
					{
						object nativeObject = directoryEntry.NativeObject;
						using (DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry))
						{
							directorySearcher.Filter = "(&(objectClass=OpenLDAPPerson))";
							directorySearcher.PropertiesToLoad.Add("uid");
							directorySearcher2.PropertiesToLoad.Add(string.Concat(Property.mail) ?? "");
							directorySearcher2.PropertiesToLoad.Add(string.Concat(Property.telephoneNumber) ?? "");
							list = new List<User>();
							foreach (SearchResult item in directorySearcher.FindAll())
							{
								User user = new User();
								using (DirectoryEntry directoryEntry2 = item.GetDirectoryEntry())
								{
									if (directoryEntry2.Properties["uid"].Value != null)
									{
										user.userLogin = directoryEntry2.Properties["uid"].Value.ToString();
									}
									if (directoryEntry2.Properties[string.Concat(Property.mail) ?? ""].Value != null)
									{
										user.userMail = directoryEntry2.Properties[string.Concat(Property.mail) ?? ""].Value.ToString();
									}
									if (directoryEntry2.Properties[string.Concat(Property.telephoneNumber) ?? ""].Value != null)
									{
										user.telephoneNumber = directoryEntry2.Properties[string.Concat(Property.telephoneNumber) ?? ""].Value.ToString();
									}
								}
								list.Add(user);
							}
						}
					}
				}
			}
			catch
			{
			}
			return list;
		}

		public User SearchUserLdap(string uid)
		{
			User user = null;
			cnLdap.Connect();
			using (DirectoryEntry searchRoot = cnLdap.entry)
			{
				using (DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot))
				{
					directorySearcher.Filter = "(cn=" + uid + ")";
					directorySearcher.PropertiesToLoad.Add(string.Concat(Property.mail) ?? "");
					directorySearcher.PropertiesToLoad.Add(string.Concat(Property.telephoneNumber) ?? "");
					SearchResult searchResult = directorySearcher.FindOne();
					if (searchResult != null)
					{
						user = new User();
						DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
						if (directoryEntry.Properties["uid"].Value != null)
						{
							user.userLogin = directoryEntry.Properties["uid"].Value.ToString();
						}
						if (directoryEntry.Properties[string.Concat(Property.mail) ?? ""].Value != null)
						{
							user.userLogin = directoryEntry.Properties[string.Concat(Property.mail) ?? ""].Value.ToString();
						}
						if (directoryEntry.Properties[string.Concat(Property.telephoneNumber) ?? ""].Value != null)
						{
							user.userLogin = directoryEntry.Properties[string.Concat(Property.telephoneNumber) ?? ""].Value.ToString();
						}
					}
				}
			}
			return user;
		}

		public bool DeleteUserLdap(string uid)
		{
			bool result = false;
			try
			{
				cnLdap.Connect();
				using (DirectoryEntry directoryEntry = cnLdap.entry)
				{
					using (DirectoryEntry entry = directoryEntry.Children.Find("uid=" + uid, "OpenLDAPPerson"))
					{
						directoryEntry.RefreshCache();
						directoryEntry.Children.Remove(entry);
						directoryEntry.CommitChanges();
						directoryEntry.Close();
						directoryEntry.Dispose();
						result = true;
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
