---
subject: Email Subject
from: no-reply@example.com
enabled: true
cc: 
bcc: admin@example.com; hello@example.com
---

This is a template for an email sent out when an event occurs.
An email will only be sent out if this file is present and valid.
The file must have the same name as the event. For example, to
send an event to the author of a blog post when somebody posts a
comment, the file 'blog.comment.reply.md' will be used as a template.

The required headers are as follows:
- subject: Template for the email subject.

The optional headers are as follows:
- from: The from address in the email. If blank, the system default will be used. This email must be present in the configured accounts.
- enabled: If false, the email will not be sent out.
- cc/bcc: Addresses to be added to cc/bcc for this template, separated with semicolons.

The subject and body of the email are templates. A template is regular text with variables
replaced by their values depending on the context. For example in the template for
'blog.comment.reply', any text '{post.title}' will be replaced by the title of the blog post.

### blog.comment.author
Used to notify a post author that somebody has commented on their post.

Available parameters are:

- {post.url}: URL to the blog post.
- {post.title}: The title of the blog post.
- {post.author}: The author of the post (recipient of this email).
- {comment.author}: The author of the comment.
- {comment.url}: Link to the comment.
- {comment.html}: Html of the comment.

### blog.comment.reply
Used to notify a commenter that somebody has replied to their comment.

Available parameters are:

- {post.url}: URL to the blog post.
- {post.title}: The title of the blog post.
- {post.author}: The author of the post.
- {comment.author}: The author of the reply.
- {comment.url}: Link to the reply.
- {comment.html}: Html of the comment.
- {parent.author}: The author of the comment (i.e the recipient of this email).
- {parent.url}: Link to the comment replied to.
