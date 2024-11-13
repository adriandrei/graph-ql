# graph-ql

## Sample Query
```
{
  users(first: 10, after: "Mg==", nameSearchKey: "3") {
    totalCount
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
    edges{
      cursor
      node{
        id
        name
        posts{
          id
          text
        }
        postsWithDataLoader{
          id
          text
        }
      }
    }
  }
}
```