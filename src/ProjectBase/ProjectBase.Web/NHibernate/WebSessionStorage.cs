﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;
using ProjectBase.Data.NHibernate;

namespace ProjectBase.Web.NHibernate
{
    public class WebSessionStorage : ISessionStorage
    {
        private const string CurrentSessionKey = "nhibernate.current_session";

        public WebSessionStorage(HttpApplication app) {
            app.EndRequest += new EventHandler(Application_EndRequest);
        }

        public ISession Session {
            get {
                HttpContext context = HttpContext.Current;
                ISession session = context.Items[CurrentSessionKey] as ISession;
                return session;
            }
            set {
                HttpContext context = HttpContext.Current;
                context.Items[CurrentSessionKey] = value;
            }
        }

        void Application_EndRequest(object sender, EventArgs e) {
            ISession session = Session;

            if (session != null) {
                session.Close();
                HttpContext context = HttpContext.Current;
                context.Items.Remove(CurrentSessionKey);
            }
        }
    } 

}