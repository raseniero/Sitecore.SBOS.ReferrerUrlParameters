﻿using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;

namespace Sitecore.Analytics.Rules.Conditions
{
  public class HasUtmTerm<T> : StringOperatorCondition<T> where T : RuleContext
  {
    public string UtmTerm { get; set; }

    protected override bool Execute(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");
      Assert.IsNotNull(Tracker.Current, "The current Tracker is not initialized");
      Assert.IsNotNull(Tracker.Current.Session, "The current Tracker.Current.Session is not initialzied");
      Assert.IsNotNull(Tracker.Current.Interaction, "The current Tracker.Current.Interaction is not initialized");
      Assert.IsNotNullOrEmpty(this.UtmTerm, "The UtmTerm is not initialized");

      //get CustomValues from Interaction
      var customValues = Tracker.Current.Interaction.CustomValues;
      
      //get UtmTermValueObj value if one exist otherwise exit method
      object UtmTermValueObj;
      if (!customValues.TryGetValue("utm_term", out UtmTermValueObj))
      {
        throw new InvalidOperationException("CustomValues does not contain specified key");
      }

      if (UtmTermValueObj == null)
      {
        return false;
      }

      string UtmTermValue = UtmTermValueObj.ToString();

      //does utmtermvalue compares?
      return this.Compare(UtmTermValue, this.UtmTerm ?? String.Empty);
    }
  }
}
