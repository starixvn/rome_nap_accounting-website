<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupddangercust">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@danger">
                  <td class="colW1">
                    {lbl:danger}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@danger" />
                  </td>
                </xsl:when>
                <xsl:when test="@danger2">
                  <td class="colW1">
                    {lbl:danger2}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@danger2" />
                  </td>
                </xsl:when>
                <xsl:when test="@danger3">
                  <td class="colW1">
                    {lbl:danger3}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@danger3" />
                  </td>
                </xsl:when>
                <xsl:when test="@danger1">
                  <td class="colW1">
                    {lbl:danger1}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@danger1" />
                  </td>
                </xsl:when>
              </xsl:choose>

              <td class="colW1">
                    {lbl:language}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
              <td class="colW1">
                {lbl:OldValue}
              </td>
              <td  class="colW2">
                <xsl:value-of select="@oldvalue" />
              </td>
            </tr>
            <tr>
              <td class="colW1">
                {lbl:isUpdateDownline}
              </td>
              <td class="colW2">
                <xsl:value-of select="@updatedownline" />
              </td>
                <td class="colW1">
                  {lbl:errCode}
                </td>
                <td  class="colW2">
                  <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">
                    {lbl:errMsg}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>

                
                
            </tr>
          <tr>          

            <td class="colW1">
              {lbl:ip}
            </td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">             
            </td>
            <td class="colW2">             
            </td>
            <td class="colW1">           
            </td>
            <td class="colW2">             
            </td>

          </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>
