<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdusername">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@fromname">
                  <td class="colW1">
                    {lbl:fromName}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@fromname" />
                  </td>
                </xsl:when>
                <xsl:when test="@oldname">
                  <td class="colW1">
                    {lbl:oldName}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@oldname" />
                  </td>
                </xsl:when>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="@toname">
                  <td class="colW1">
                    {lbl:toName}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@toname" />
                  </td>
                </xsl:when>
                <xsl:when test="@newname">
                  <td class="colW1">
                    {lbl:newName}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@newname" />
                  </td>
                </xsl:when>
              </xsl:choose>


              <xsl:choose>
                <xsl:when test="@ip">
                  <td class="colW1">{lbl:IP}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ip" />
                  </td>
                </xsl:when>
                <xsl:when test="@returnerrcode">
                  <td class="colW1">{lbl:ReturnErrCode}</td>
                  <td class="colW2">
                    <xsl:value-of select="@returnerrcode" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>
            </tr>
            

            <tr>
                <td class="colW1">
                    {lbl:errCode}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">
                    {lbl:errMsg}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>
