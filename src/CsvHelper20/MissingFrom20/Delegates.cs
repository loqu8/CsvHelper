﻿// Copyright 2009-2014 Josh Close and Contributors
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
namespace CsvHelper.MissingFrom20
{
	/// <summary>
	/// Encapsulates a method that has one parameter and returns a value of the type specified by the TResult parameter.
	/// </summary>
	/// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	/// <param name="arg">The parameter of the method that this delegate encapsulates.</param>
	/// <returns>The return value of the method that this delegate encapsulates.</returns>
	public delegate TResult Func<in T, out TResult>( T arg );
}
