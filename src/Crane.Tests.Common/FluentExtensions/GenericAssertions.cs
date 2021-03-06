﻿using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Crane.Tests.Common.FluentExtensions
{
    public class GenericAssertions<T> : ReferenceTypeAssertions<T, GenericAssertions<T>> where T : class 
    {
        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// 
        /// </summary>
        protected override string Context
        {
            get { return typeof (T).Name; }
        }

        public GenericAssertions(T value)
        {
            this.Subject = value;
        }

        /// <summary>
        /// Asserts that an object equals another object using its <see cref="M:System.Object.Equals(System.Object)"/> implementation.
        /// 
        /// </summary>
        /// <param name="expected">The expected value</param><param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        ///             </param><param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.
        ///             </param>
        public AndConstraint<GenericAssertions<T>> Be(T expected, string because = "", params object[] reasonArgs)
        {
            using (var assertionScope = Execute.Assertion)
            {
                assertionScope.BecauseOf(because, reasonArgs).ForCondition(this.Subject.Equals(expected)).FailWith("Expected {context:object} to be {0}{reason}, but found {1}.", expected, this.Subject);                
            }

            return new AndConstraint<GenericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="M:System.Object.Equals(System.Object)"/> method.
        /// 
        /// </summary>
        /// <param name="unexpected">The unexpected value</param><param name="because">A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        ///             start with the word <i>because</i>, it is prepended to the message.
        ///             </param><param name="reasonArgs">Zero or more values to use for filling in any <see cref="M:System.String.Format(System.String,System.Object[])"/> compatible placeholders.
        ///             </param>
        public AndConstraint<GenericAssertions<T>> NotBe(T unexpected, string because = "", params object[] reasonArgs)
        {
            using (var assertionScope = Execute.Assertion)
            {
                assertionScope.ForCondition(!this.Subject.Equals(unexpected)).BecauseOf(because, reasonArgs).FailWith("Did not expect {context:object} to be equal to {0}{reason}.", new object[1]
                {
                    unexpected
                });
            }

            return new AndConstraint<GenericAssertions<T>>(this);
        }
    }
}