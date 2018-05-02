using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace CodeBase2
{
    public static class EntityFrameworkContext
    {
        public static void LoadMetadataFromAssembly(this ObjectContext context)
        {
            context.MetadataWorkspace.LoadFromAssembly(context.GetType().Assembly);
        }

        public static void AttachUpdated(this ObjectContext context, EntityObject detachedEntity)
        {
            if (detachedEntity.EntityState == EntityState.Detached)
            {
                object currentEntity;

                if (context.TryGetObjectByKey(detachedEntity.EntityKey, out currentEntity))
                {
                    context.ApplyPropertyChanges(detachedEntity.EntityKey.EntitySetName, detachedEntity);

                    var newEntity = detachedEntity as IEntityWithRelationships;
                    var oldEntity = currentEntity as IEntityWithRelationships;

                    if (newEntity != null && oldEntity != null)
                    {
                        context.ApplyReferencePropertyChanges(newEntity, oldEntity);
                    }
                }
                else
                {
                    throw new ObjectNotFoundException();
                }
            }
        }

        private static void ApplyReferencePropertyChanges(this ObjectContext context, IEntityWithRelationships newEntity, IEntityWithRelationships oldEntity)
        {
            foreach (var relatedEnd in oldEntity.RelationshipManager.GetAllRelatedEnds())
            {
                var oldReference = relatedEnd as EntityReference;
                
                if (oldReference != null)
                {
                    var newReference = newEntity.RelationshipManager.GetRelatedEnd(oldReference.RelationshipName,
                        oldReference.TargetRoleName) as EntityReference;

                    if (newReference != null)
                    {
                        oldReference.EntityKey = newReference.EntityKey;
                    }
                }
            }
        }

       
    }
}
