<%@ Control Language="C#" AutoEventWireup="false" CodeFile="StudentLaminateView.ascx.cs" Inherits="wctMain.View.StudentLaminateView" %>

<div class="displayer fade fade-slow laminate-container static-container">
    <div class="section-header">
        Student Laminates        
        <% if(context.ToLower() == "mobile"){ %>
            <a class="btn btn-primary btn-xs pull-right lammie-close" id="laminate-close-top" href="/" title="Close">Close</a>
        <%} %>
    </div>
    <div class="main-inner">
        <h3 style="display: block; text-align: center; margin-bottom: 15px;">CU Students!  Love Live Music?</h3>
        <p>
            Want access to all shows at the Fox Theatre & Boulder Theater during the 2014-2015 school year!
            <br />
            Now's your chance!
        </p>
        <p>
            Purchase the STUDENT LAMINATE for $500 and receive one ticket to the shows at the Fox Theatre & Boulder Theater during the 2014 - 2015 school year!  Plus, receive a FREE Fox Theatre or Boulder Theater piece of merchandise upon purchase!
        </p>
        <p>
            <span style="display:block;font-size:18px;">
                <a style="text-decoration: underline; color: #ff8933;" href="http://bouldertheater.frontgatesolutions.com/choose.php?a=1&lid=97527&eid=108962">Click Here</a> to purchase before they are all gone!
            </span>
            Limited Availability 
        </p>
        <p>
            For inquiries, please contact the Fox Theatre at 303-447-2461 or email us at <a style="text-decoration: underline;font-weight:bold; color: #428bca;" href="mailto:boxofficeasst@z2ent.com?subject=student laminate 2014-2015">boxofficeasst@z2ent.com</a>
        </p>
        <p>
            <small>**NOT Valid for sold out seated events, private rental events, or events promoted by entities other than Z2 Entertainment.
            <br />
            **Must have valid ID with laminate upon entry
            <br />
            **Non-transferable            
            </small>
        </p>

        <% if(context.ToLower() == "mobile"){ %>
        <p><br /><a class="btn btn-primary btn-xs lammie-close" href="/" title="Close">Close</a></p>
        <%} %>
    </div>
</div>
