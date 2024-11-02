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
    nodes {
      id
      name
      posts {
        id
        text
      }
    }
  }
}
```