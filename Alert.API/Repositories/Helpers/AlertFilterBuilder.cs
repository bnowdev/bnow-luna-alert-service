using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Alert.API.Extensions;
using Microsoft.EntityFrameworkCore;


namespace Alert.API.Repositories.Helpers
{
    public static class AlertFilterBuilder
    {
        private static readonly string[] _filterOperators =
        {
            "IS",
            "ISNOT",
            "CONTAINS",
            "NOTCONTAINS",
            "STARTSWITH",
            "ENDSWITH",
            "EQUALS",
            "NOTEQUALS",
            "ISLESSTHAN",
            "ISMORETHAN",
            "ISMORETHANOREQUAL",
            "ISLESSTHANOREQUAL",
            "ON",
            "NOTON",
            "AFTER",
            "BEFORE",
            "ONORAFTER",
            "ONORBEFORE"
        };

        #region Filtered IQueryable methods

        public static IQueryable<Models.Alert> GetFilteredQueryable(IQueryable<Models.Alert> alerts, string query)
        {
            var filterList = GetFilterList(query);

            if (filterList == null || filterList.Count < 1)
            {
                return null;
            }

            var predicate = PredicateBuilder.True<Models.Alert>();

            // Combine all filters into one expression (predicate) used for filtering IQueryable alerts.
            foreach (var filter in filterList)
            {
                predicate = GetAppendedFiltersPredicate(predicate, filter);
            }

            // Return IQueryable with added filters in the Where method.
            return alerts.Where(predicate);
        }

        private static Expression<Func<Models.Alert, bool>> GetAppendedFiltersPredicate(
            Expression<Func<Models.Alert, bool>> predicate, Filter filter)
        {
            if (filter.Type == FilterType.OR)
            {
                return predicate.Or(GetFilterExpression(filter));
            }

            if (filter.Type == FilterType.AND)
            {
                return predicate.And(GetFilterExpression(filter));
            }

            throw new InvalidOperationException("Unrecognized filter condition type. Accepted types: AND, OR");
        }

        private static Expression<Func<Models.Alert, bool>> GetFilterExpression(Filter filter)
        {
            if (!String.IsNullOrEmpty(filter.StringValue))
            {
                switch (filter.Operator)
                {
                    case "IS":
                        return a => EF.Property<string>(a, filter.Field) == filter.StringValue;

                    case "ISNOT":
                        return a => EF.Property<string>(a, filter.Field) != filter.StringValue;

                    case "CONTAINS":
                        return a => EF.Property<string>(a, filter.Field).Contains(filter.StringValue);

                    case "NOTCONTAINS":
                        return a => !EF.Property<string>(a, filter.Field).Contains(filter.StringValue);

                    case "STARTSWITH":
                        return a => EF.Property<string>(a, filter.Field).StartsWith(filter.StringValue);

                    case "ENDSWITH":
                        return a => EF.Property<string>(a, filter.Field).EndsWith(filter.StringValue);
                }
            }
            else if (filter.DateValue != null)
            {

                switch (filter.Operator)
                {
                    case "ON":
                        return a => EF.Property<DateTime>(a, filter.Field) == (filter.DateValue).Value;

                    case "NOTON":
                        return a => EF.Property<DateTime>(a, filter.Field) != (filter.DateValue).Value;

                    case "AFTER":
                        return a => EF.Property<DateTime>(a, filter.Field) > (filter.DateValue).Value;

                    case "BEFORE":
                        return a => EF.Property<DateTime>(a, filter.Field) < (filter.DateValue).Value;

                    case "ONORAFTER":
                        return a => EF.Property<DateTime>(a, filter.Field) >= (filter.DateValue).Value;

                    case "ONORBEFORE":
                        return a => EF.Property<DateTime>(a, filter.Field) <= (filter.DateValue).Value;
                }
            }
            else if (filter.NumericValue != null)
            {
                switch (filter.Operator)
                {
                    case "EQUALS":
                        return a => EF.Property<int>(a, filter.Field) == filter.NumericValue;

                    case "NOTEQUALS":
                        return a => EF.Property<int>(a, filter.Field) != filter.NumericValue;

                    case "ISMORETHAN":
                        return a => EF.Property<int>(a, filter.Field) > (filter.NumericValue);

                    case "ISLESSTHAN":
                        return a => EF.Property<int>(a, filter.Field) < (filter.NumericValue);

                    case "ISMORETHANOREQUAL":
                        return a => EF.Property<int>(a, filter.Field) >= (filter.NumericValue);

                    case "ISLESSTHANOREQUAL":
                        return a => EF.Property<int>(a, filter.Field) <= (filter.NumericValue);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(filter), "The filter value cannot be null");
            }

            return null;
        }

        #endregion


        #region Filter List methods

        // Creates a list of filter objects used for building a filtered IQueryable DbSet<Alert>.
        private static IList<Filter> GetFilterList(string query)
        {
            if (!String.IsNullOrWhiteSpace(query))
            {
                IList<Filter> filterList = new List<Filter>();

                // Split query into separate filter conditions string[] by '^' delimiter.
                var conditions = query.Split('^');

                // For each condition is created a Filter object with AND || OR filterType. 
                // If its a first condition it should be always AND.
                // Filters are added to list used for created a filtered IQueryable on 
                foreach (var condition in conditions)
                {   
                    var filter = GetFilter(condition);
                    filter.Type = GetConditionFilterType(condition);
                  //  if(filter.Type == FilterType.OR) 
                    filterList.Add(filter);
                }

                return filterList;
            }

            return null;
        }

        // Return the Filter bbject based on a single condition from the query
        private static Filter GetFilter(string condition)
        {
            // Split condition to field, operator, value 
            var conditionArray = condition.Split("__");

            if (conditionArray.Length < 3)
            {
                if (conditionArray.Length < 2)
                {
                    if (conditionArray.Length < 1)
                    {
                        throw new ArgumentNullException(nameof(condition), "The condition missing: field, operator, value");

                    }
                    throw new ArgumentNullException(nameof(condition), "The condition missing: operator, value");
                }

                throw new ArgumentNullException(nameof(condition), "The condition missing: value");
            }

            string field = conditionArray[0];
            string conditionOperator = conditionArray[1];
            string value = conditionArray[2];

            // Get condition type
            // If OR then delete the 2 first to chars from string value to condition value
            var filterType = GetConditionFilterType(condition);
            if (filterType == FilterType.OR)
            {
                field = field.Remove(0, 2);
            }

            // Convert from json camelCase for iterating later over DbSet<Alert>
            field = field.ToPascalCase();


            if (!String.IsNullOrEmpty(conditionOperator))
            {
                switch (conditionOperator)
                {
                    case "IS":
                    case "ISNOT":
                    case "CONTAINS":
                    case "STARTSWITH":
                    case "ENDSWITH":
                        return new Filter()
                        {
                            Field = field,
                            Operator = conditionOperator,
                            StringValue = value
                        };

                    case "ON":
                    case "NOTON":
                    case "AFTER":
                    case "BEFORE":
                    case "ONORAFTER":
                    case "ONORBEFORE":
                        var now = DateTime.Now;
                        var date = DateTime.Parse(value);
                        return new Filter()
                        {
                            Field = field,
                            Operator = conditionOperator,
                            DateValue = DateTime.Parse(value)
                        };

                    case "EQUALS":
                    case "NOTEQUALS":
                    case "ISLESSTHAN":
                    case "ISMORETHAN":
                    case "ISMORETHANOREQUAL":
                    case "ISLESSTHANOREQUAL":
                        return new Filter()
                        {
                            Field = field,
                            Operator = conditionOperator,
                            NumericValue = int.Parse(value)
                        };
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(conditionOperator),
                    "The filter condition operator cannot be null");
            }

            return null;
        }

        private static FilterType GetConditionFilterType(string condition)
        {
            return condition.StartsWith("OR") ? FilterType.OR : FilterType.AND;
        }

       
       

        #endregion
    }
}