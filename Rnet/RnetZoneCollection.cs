﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Rnet
{

    /// <summary>
    /// Holds a collection of zones.
    /// </summary>
    public sealed class RnetZoneCollection : IEnumerable<RnetZone>, INotifyCollectionChanged
    {

        ConcurrentDictionary<RnetZoneId, WeakReference<RnetZone>> zones =
            new ConcurrentDictionary<RnetZoneId, WeakReference<RnetZone>>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal RnetZoneCollection(RnetController controller)
        {
            Controller = controller;
        }

        /// <summary>
        /// Controller holding this zone collection.
        /// </summary>
        RnetController Controller { get; set; }

        /// <summary>
        /// Gets a zone with the given identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RnetZone this[RnetZoneId id]
        {
            get { return GetOrCreate(id); }
        }

        /// <summary>
        /// Gets or creates a new zone object for the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RnetZone GetOrCreate(RnetZoneId id)
        {
            if (RnetZoneId.IsReserved(id))
                return null;

            return zones
                .GetOrAdd(id, i => new WeakReference<RnetZone>(new RnetZone(Controller, id)))
                .GetTargetOrDefault();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the known zones.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RnetZone> GetEnumerator()
        {
            return zones.Values
                .Select(i => i.GetTargetOrDefault())
                .Where(i => i != null)
                .Where(i => i.IsActive)
                .OrderBy(i => i.Id)
                .ToList()
                .GetEnumerator();
        }

        /// <summary>
        /// Invoked when a zone becomes active.
        /// </summary>
        /// <param name="zone"></param>
        internal void OnZoneActive(RnetZone zone)
        {
            Controller.Touch();
            RaiseCollectionChanged();
        }

        /// <summary>
        /// Raised when zones are added or removed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        void RaiseCollectionChanged()
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
